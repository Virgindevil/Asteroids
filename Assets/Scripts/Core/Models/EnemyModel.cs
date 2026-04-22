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

        public abstract void Update(float dt);

        public virtual void TakeDamage(float amount)
        {
            if (IsDead) return;
            Health -= amount;
            Debug.Log($"[Enemy Log] {Config.EnemyType} damaged. HP: {Health}");
            if (IsDead) Debug.Log($"[Enemy Log] {Config.EnemyType} is DEAD!");
        }

        public virtual void OnCollision(ICollidable other) 
        {
            if (other is BulletModel)
            {
                // Если лога ниже нет в консоли при стрельбе в НЛО — значит коллизия не сработала
                Debug.Log($"[Collision] {Config.EnemyType} hit by Bullet!");
                TakeDamage(1f);
            }
            else
            {
                PhysicsBody.ResolvePushApart(this, other);
            }
        }
    }
}