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

        public override void Update(float dt)
        {
            Body.UpdatePhysics(dt);
        }

        public List<FragmentData> GetFragments()
        {
            var fragments = new List<FragmentData>();
            if (!CanSplit) return fragments;

            int count = 3; // По ТЗ 3 осколка
            float fragmentRadius = CollisionRadius * 0.5f;

            for (int i = 0; i < count; i++)
            {
                // Генерируем случайное направление разлета
                Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

                fragments.Add(new FragmentData
                {
                    Position = Body.Position + (randomDir * fragmentRadius),
                    // Осколки летят чуть быстрее в разные стороны
                    Velocity = (Body.Velocity * 0.5f) + (randomDir * 3f),
                    Radius = fragmentRadius
                });
            }

            return fragments;
        }
    }
}
