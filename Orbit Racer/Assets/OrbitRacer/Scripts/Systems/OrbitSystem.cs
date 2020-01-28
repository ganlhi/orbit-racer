using OrbitRacer.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace OrbitRacer.Systems
{
    [DisableAutoCreation]
    public class OrbitSystem: JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            var elapsedTime = deltaTime * SimulationSettings.Instance.timeScale;

            return Entities.ForEach((ref Translation translation, in Orbit orbit) =>
            {
                var degreesPerSecond = 360f / orbit.Period;
                var angle = math.radians(degreesPerSecond * elapsedTime);
                var rotation = quaternion.RotateZ(angle);
                var dist = orbit.Radius; 
                translation.Value = math.mul(rotation, math.normalize(translation.Value) * dist);
            }).Schedule(inputDeps);
        }
    }
}