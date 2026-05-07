using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Core
{
    public struct FragmentData
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Radius;
    }

    public class AsteroidModel : EnemyModel
    {
        public bool CanSplit { get; private set; }

        public AsteroidModel(EnemyConfig config, Vector2 pos, Vector2 vel)
            : base(config, pos, vel)
        {
            CanSplit = config.CanSplit;
        }

        public override void Update(float dt,  float frictionMultiplier)
        {
            Body.UpdatePhysics(dt, frictionMultiplier);
        }

        public List<FragmentData> GetFragments()
        {
            var fragments = new List<FragmentData>();
            if (!CanSplit) return fragments;

            float fragmentRadius = CollisionRadius * Config.FragmentRadiusMultiplier;

            for (int i = 0; i < Config.Fragments; i++)
            {
                Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

                fragments.Add(new FragmentData
                {
                    Position = Body.Position + (randomDir * fragmentRadius),
                    Velocity = (Body.Velocity * Config.FragmentRadiusMultiplier) + (randomDir * Config.FragmentSpeedBoost),
                    Radius = fragmentRadius
                });
            }

            return fragments;
        }
    }
}
