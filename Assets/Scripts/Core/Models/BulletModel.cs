using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public class BulletModel : ICollidable
    {
        public PhysicsBody Body { get; }
        public bool IsActive { get; set; } = true; // Теперь сеттер публичный для менеджера
        public float LifeTime { get; set; } // Добавили это поле

        public float CollisionRadius => 0.1f;

        public BulletModel(Vector2 pos, Vector2 direction, float lifeDuration = 2f)
        {
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = direction * 15f;
            LifeTime = lifeDuration; // Устанавливаем время жизни при создании
        }

        public void OnCollision(ICollidable other)
        {
            if (other is EnemyModel)
            {
                IsActive = false;
            }
        }
    }
}