using UnityEngine;

namespace Game.Core
{
    public class PlayerModel
    {
        public PhysicsBody Body { get; private set; }
        public PlayerConfig Config { get; private set; }

        public int Health { get; private set; }

        public PlayerModel(PlayerConfig config)
        {
            Config = config;
            Health = (int)config.MaxHealth;

            // Создаем тело. Центр мира (0,0), трение из JSON
            Body = new PhysicsBody(Vector2.zero, config.Friction);
        }

        public void Accelerate(Vector2 direction, float deltaTime)
        {
            // Сила = Направление * Ускорение * Время
            Vector2 force = direction.normalized * Config.MovementAcceleration * deltaTime;
            Body.AddForce(force);
        }
    }
}