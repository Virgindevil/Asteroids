using UnityEngine;

namespace Game.Core
{
    public class BulletModel : ICollidable
    {
        public BulletModel(Vector2 pos, Vector2 velocity, BulletSettings settings, float lifeDuration = 2f)
        {
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = velocity;
            LifeTime = lifeDuration;
            CollisionRadius = settings.CollisionRadius;
            Damage = settings.BulletDamage;
        }

        public bool IsActive { get; set; } = true;
        public float LifeTime { get; set; }
        public int Damage { get; }
        public PhysicsBody Body { get; }
        public float CollisionRadius { get; }

        public void OnCollision(ICollidable other)
        {
            if (other is EnemyModel)
                IsActive = false;
        }
    }
}