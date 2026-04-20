using UnityEngine;

namespace Game.Core
{
    // Используем abstract, чтобы нельзя было создать "просто врага" 
    // без конкретной реализации (Астероид или НЛО)
    public abstract class EnemyModel : ICollidable
    {
        // Общие компоненты для всех врагов
        public PhysicsBody Body { get; }
        public EnemyConfig Config { get; }
        
        // Реализация интерфейса ICollidable
        public float CollisionRadius => Config.CollisionRadius;

        protected EnemyModel(EnemyConfig config, Vector2 position, Vector2 velocity)
        {
            Config = config;
            // Трение берем из конфига (1.0 для астероидов, 0.9 для НЛО)
            Body = new PhysicsBody(position, config.Friction);
            Body.Velocity = velocity;
        }

        // Этот метод ОБЯЗАНЫ реализовать все наследники
        public abstract void Update(float dt);

        // Виртуальный метод для столкновений (можно переопределить при желании)
        public virtual void OnCollision(ICollidable other) 
        {
            if (other is EnemyModel otherEnemy)
            {
                // Враг-враг: отталкивание без урона
                Body.ResolvePushApart(this, otherEnemy);
                Debug.Log($"[Enemy] Collision: {Config.EnemyType} ↔ {otherEnemy.Config.EnemyType}");
            }
            else if (other is PlayerModel player)
            {
                // Враг-игрок: отталкивание + лог
                Body.ResolvePushApart(this, player);
                Debug.Log($"[Enemy] Collision with PLAYER at {Body.Position}");
            }
        }
    }
}