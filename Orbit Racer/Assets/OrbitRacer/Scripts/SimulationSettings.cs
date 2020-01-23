using System;
using UnityEngine;

namespace OrbitRacer
{
    [Serializable]
    [CreateAssetMenu]
    public class SimulationSettings: ScriptableObject
    {
        [Tooltip("Real seconds per game second")]
        public float timeScale;

        public float kmPerWorldUnit;

        [Tooltip("How many seconds in the future to predict movements")]
        public float predictionsLeadTime;

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