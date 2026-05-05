using UnityEngine;

namespace Game.Core
{
    public class BulletModel : ICollidable
    {
        public PhysicsBody Body { get; }
        public bool IsActive { get; set; } = true;
        public float LifeTime { get; set; }
        public float CollisionRadius { get; } 
        public int Damage { get; }

        public BulletModel(Vector2 pos, Vector2 velocity, BulletSettings settings, float lifeDuration = 2f)
        {
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = velocity;
            LifeTime = lifeDuration;
            CollisionRadius = settings.CollisionRadius;
            Damage = settings.BulletDamage;
        }

        public void OnCollision(ICollidable other)
        {
            if (other is EnemyModel)
                IsActive = false;
        }
    }
}