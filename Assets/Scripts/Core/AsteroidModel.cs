using UnityEngine;

namespace Game.Core
{
    public class AsteroidModel : ICollidable
    {
        public PhysicsBody Body { get; }
        public EnemyConfig Config { get; }
        public float CollisionRadius => Config.CollisionRadius;

        public AsteroidModel(EnemyConfig config, Vector2 position, Vector2 velocity)
        {
            Config = config;
            // Трение для астероидов обычно ставим 1.0 (без затухания скорости), 
            // чтобы они летели равномерно сквозь космос
            Body = new PhysicsBody(position, Config.Friction);
            Body.Velocity = velocity;
        }

        public void Update(float dt)
        {
            Body.UpdatePhysics(dt);
        }

        public void OnCollision(ICollidable other)
        {
            // Логика уничтожения будет обрабатываться через менеджер или пулы
        }
    }
}