using System;
using System.Collections.Generic;
using OrbitRacer.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

namespace OrbitRacer.MonoBehaviours
{
    public class SolarSystem : MonoBehaviour
    {
        [Serializable]
        public struct PlanetSetup
        {
            public string name;
            public Orbit orbit;
            public Material material;
            public float scale;
        }

#pragma warning disable 649
        [SerializeField] private List<PlanetSetup> planets = new List<PlanetSetup>();
        [SerializeField] private Mesh mesh;
        [SerializeField] private float baseScale = .3f;
        [SerializeField] private float baseRadius = 2f;
        [SerializeField] private float basePeriod = 20f;
        [SerializeField] private string startingPlanet = "Earth";
        [SerializeField] private float startingDistance = 1f;
        [SerializeField] private Material shipMaterial;
        [SerializeField] private float shipScale = 1f;
        [SerializeField] private float shipMass = .0001f;
#pragma warning restore 649

        private void Start()
        {
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var rng = new Random((uint) UnityEngine.Random.Range(int.MinValue, int.MaxValue));

            var startingPlanetPosition = float3.zero;

            foreach (var planet in planets)
            {
                var entity = em.CreateEntity(typeof(RenderMesh), typeof(LocalToWorld));

                em.SetSharedComponentData(entity, new RenderMesh
                {
                    mesh = mesh,
                    material = planet.material,
                });

                em.AddComponentData(entity, new NonUniformScale
                {
                    Value = math.float3(planet.scale * baseScale)
                });

                em.AddComponentData(entity, new Mass
                {
                    Value = planet.scale * baseScale
                });

                var translation = new Translation();

                var orbitRadius = planet.orbit.Radius * baseRadius;
                if (orbitRadius > float.Epsilon)
                {
                    var secondsPeriod = planet.orbit.Period * basePeriod;
                    var randAngle = rng.NextFloat() * 360f;
                    var rotation = quaternion.RotateZ(randAngle);
                    var position = math.mul(rotation, math.normalize(math.up()) * orbitRadius);

                    translation.Value = position;

                    em.AddComponentData(entity, new Orbit
                    {
                        Period = secondsPeriod,
                        Radius = orbitRadius
                    });
                }

                em.AddComponentData(entity, translation);

                if (planet.name == startingPlanet)
                {
                    startingPlanetPosition = translation.Value;
                }

                em.SetName(entity, planet.name);
            }

            // Place ship
            var ship = em.CreateEntity(typeof(RenderMesh), typeof(LocalToWorld));

            em.SetSharedComponentData(ship, new RenderMesh
            {
                mesh = mesh,
                material = shipMaterial,
            });

            var s = shipScale * baseScale;
            em.AddComponentData(ship, new NonUniformScale
            {
                Value = math.float3(s * .5f, s * 1f, s * 1f)
            });

            em.AddComponentData(ship, new Mass
            {
                Value = shipMass
            });

            var randomShipAngle = math.radians(math.lerp(-90f, 90f, rng.NextFloat()));
            var offset = math.mul(quaternion.RotateZ(randomShipAngle),
                math.normalize(startingPlanetPosition) * startingDistance);
            var shipPosition = startingPlanetPosition + offset;

            em.AddComponentData(ship, new Translation
            {
                Value = shipPosition
            });
        }
    }
}