using OrbitRacer.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace OrbitRacer.Systems
{
    [DisableAutoCreation]
    public class GravitySystem : JobComponentSystem
    {
        private struct Attractor
        {
            public float3 Position;
            public float Mass;
        }

        [RequireComponentTag(typeof(Ship))]
        private struct AttractorsJob : IJobForEach<Movement, Mass, Translation>
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Attractor> Attractors;
            public float G;

            public void Execute(ref Movement movement, [ReadOnly] ref Mass mass, [ReadOnly] ref Translation translation)
            {
                foreach (var attractor in Attractors)
                {
                    var force = G * (mass.Value * attractor.Mass) /
                                math.pow(
                                    math.distance(translation.Value, attractor.Position), 
                                    2
                                    );

                    var acc = math.normalize(attractor.Position - translation.Value) * (force / mass.Value);
                    movement.Acceleration += acc;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var query = GetEntityQuery(
                ComponentType.Exclude<Ship>(),
                ComponentType.ReadOnly<Mass>(),
                ComponentType.ReadOnly<Translation>());

            var masses = query.ToComponentDataArray<Mass>(Allocator.TempJob);
            var translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);

            var attractors = new NativeArray<Attractor>(masses.Length, Allocator.TempJob);

            for (var i = 0; i < masses.Length; i++)
            {
                attractors[i] = new Attractor
                {
                    Mass = masses[i].Value,
                    Position = translations[i].Value
                };
            }

            masses.Dispose();
            translations.Dispose();

            var job = new AttractorsJob
            {
                Attractors = attractors,
                G = SimulationSettings.Instance.gravityConstant
            };

            var handle = job.Schedule(this, inputDeps);

            return handle;
        }
    }
}