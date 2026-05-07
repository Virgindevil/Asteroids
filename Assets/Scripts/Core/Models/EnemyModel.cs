using UnityEngine;

namespace Game.Core
{
    public abstract class EnemyModel : ICollidable
    {
        public PhysicsBody Body { get; }
        public EnemyConfig Config { get; }
        public float Health { get; protected set; }
        public bool IsDead => Health <= 0;
        
        public float CollisionRadius => Config.CollisionRadius;

        protected EnemyModel(EnemyConfig config, Vector2 position, Vector2 velocity)
        {
            Config = config;
            Health = config.Health > 0 ? config.Health : (config.CanSplit ? 2f : 1f); 

            Body = new PhysicsBody(position, config.Friction);
            Body.Velocity = velocity;
        }

        public abstract void Update(float dt, float frictionMultiplier);

        public virtual void TakeDamage(float amount)
        {
            if (IsDead) return;
            Health -= amount;
        }

        public virtual void OnCollision(ICollidable other) 
        {
            if (other is BulletModel bullet)
            {
                TakeDamage(bullet.Damage);
            }
            else { PhysicsBody.ResolvePushApart(this, other); }
        }
    }
}