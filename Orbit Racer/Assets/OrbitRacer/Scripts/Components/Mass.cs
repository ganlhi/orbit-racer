using Unity.Entities;

namespace OrbitRacer.Components
{
    [GenerateAuthoringComponent]
    public struct Mass : IComponentData
    {
        public float Value;
    }
}