using System;
using Unity.Mathematics;
using UnityEngine;

namespace OrbitRacer
{
    [Serializable]
    [CreateAssetMenu]
    public class SimulationSettings: ScriptableObject
    {
        [Tooltip("Real seconds per game second")]
        public float timeScale = 1;

        [Tooltip("How many seconds in the future to predict movements")]
        public float predictionsLeadTime = 10;

        [Tooltip("Gravity constant")]
        public float gravityConstant = 6.67f * math.pow(10, -11);

        public static SimulationSettings Instance;

        public SimulationSettings()
        {
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
        }
    }
}