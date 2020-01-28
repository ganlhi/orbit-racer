using Unity.Entities;

namespace OrbitRacer.Components
{
    [GenerateAuthoringComponent]
    public struct Fuel : IComponentData
    {
        public float Amount;
    }
}