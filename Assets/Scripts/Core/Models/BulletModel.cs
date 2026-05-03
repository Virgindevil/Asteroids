using UnityEngine;

namespace Game.Core
{
    public class BulletModel : ICollidable
    {
        public PhysicsBody Body { get; }
        public bool IsActive { get; set; } = true;
        public float LifeTime { get; set; }
        public float CollisionRadius => 0.1f;

        // Убираем direction и хардкод 15f — скорость передаётся готовым вектором
        public BulletModel(Vector2 pos, Vector2 velocity, float lifeDuration = 2f)
        {
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = velocity; // вместо direction * 15f
            LifeTime = lifeDuration;
        }

        public void OnCollision(ICollidable other)
        {
            if (other is EnemyModel)
                IsActive = false;
        }
    }
}