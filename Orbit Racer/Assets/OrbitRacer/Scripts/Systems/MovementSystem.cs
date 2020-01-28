using OrbitRacer.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace OrbitRacer.Systems
{
    public class MovementSystem: JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var elapsedTime = Time.DeltaTime * SimulationSettings.Instance.timeScale;

            return Entities.ForEach((ref Translation translation, ref Movement movement) =>
                {
                    translation.Value += elapsedTime * movement.Velocity;
                    movement.Velocity += elapsedTime * movement.Acceleration;
                }).Schedule(inputDeps);
        }
    }
}