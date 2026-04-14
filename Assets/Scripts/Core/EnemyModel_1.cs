using UnityEngine;

namespace Game.Core
{
    public class EnemyModel : global::EnemyModel, ICollidable
    {
        public PhysicsBody Body { get; }
        public EnemyConfig Config { get; }
        public float CollisionRadius => Config.CollisionRadius;

        public EnemyModel(EnemyConfig config, Vector2 pos, Vector2 vel)
        : base(config, pos, vel)
        {
            Config = config;
            // Трение для астероидов обычно ставим 1.0 (без затухания скорости), 
            // чтобы они летели равномерно сквозь космос
            Body = new PhysicsBody(pos, Config.Friction);
            Body.Velocity = vel;
        }

        public override void Update(float dt)
        {
            Body.UpdatePhysics(dt);
        }

        public override void OnCollision(ICollidable other)
        {
            // Логика уничтожения будет обрабатываться через менеджер или пулы
        }
    }
}