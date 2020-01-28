using Unity.Entities;
using Unity.Mathematics;

namespace OrbitRacer.Components
{
    [GenerateAuthoringComponent]
    public struct Movement : IComponentData
    {
        public float3 Velocity;
        public float3 Acceleration;
    }
}