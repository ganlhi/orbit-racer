using System;
using Unity.Entities;

namespace OrbitRacer.Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Orbit : IComponentData
    {
        public float Radius;
        public float Period;
    }
}