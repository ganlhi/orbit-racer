using Unity.Entities;

namespace OrbitRacer.Components
{
    [GenerateAuthoringComponent]
    public struct Engine : IComponentData
    {
        public bool Thrusting;
        public float AccelerationMagnitude;
    }
}