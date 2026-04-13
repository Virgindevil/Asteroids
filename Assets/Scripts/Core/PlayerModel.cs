using UnityEngine;

namespace Game.Core
{
    public class PlayerModel
    {
        public PhysicsBody Body { get; private set; }
        public PlayerConfig Config { get; private set; }

        // ТЕПЕРЬ ОПЕРИРУЕМ ЗАРЯДАМИ
        public float LaserCharge { get; private set; } // От 0 до MaxLaserCharges
        public bool IsLaserActive { get; set; }

        public int Health { get; private set; }

        public PlayerModel(PlayerConfig config)
        {
            Config = config;
            Health = (int)config.MaxHealth;
            Body = new PhysicsBody(Vector2.zero, config.Friction);

            // Инициализация при спавне
            LaserCharge = config.MaxLaserCharges;
        }

        public void Accelerate(Vector2 direction, float deltaTime)
        {
            // Сила = Направление * Ускорение * Время
            Vector2 force = direction.normalized * Config.MovementAcceleration * deltaTime;
            Body.AddForce(force);
        }
        public void UpdateLaser(float dt)
        {
            // Только восстанавливаем заряд, если он не полон
            if (!IsLaserActive && LaserCharge < Config.MaxLaserCharges)
            {
                // Скорость восстановления: 1 заряд за Config.LaserCooldown секунд
                LaserCharge += dt / Config.LaserCooldown;

                if (LaserCharge > Config.MaxLaserCharges)
                    LaserCharge = Config.MaxLaserCharges;
            }
        }

        public bool TryConsumeCharge()
        {
            if (LaserCharge >= 1f)
            {
                LaserCharge -= 1f;
                return true;
            }
            return false;
        }

    }
}