using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public struct FragmentData
    {
        public Vector2 Position;
        public Vector2 Velocity;
    }

    public class AsteroidModel : EnemyModel
    {
        public AsteroidModel(EnemyConfig config, Vector2 pos, Vector2 vel)
            : base(config, pos, vel)
        {
            CanSplit = config.CanSplit;
        }

        public bool CanSplit { get; }

        public override void Update(float dt, float frictionMultiplier)
        {
            Body.UpdatePhysics(dt, frictionMultiplier);
        }

        public List<FragmentData> GetFragments()
        {
            var fragments = new List<FragmentData>();
            if (!CanSplit) return fragments;

            var fragmentRadius = CollisionRadius * Config.FragmentRadiusMultiplier;

            for (var i = 0; i < Config.Fragments; i++)
            {
                var randomDir = Random.insideUnitCircle.normalized;

                fragments.Add(new FragmentData
                {
                    Position = Body.Position + randomDir * fragmentRadius,
                    Velocity = Body.Velocity * Config.FragmentRadiusMultiplier + randomDir * Config.FragmentSpeedBoost
                });
            }

            return fragments;
        }
    }
}