using UnityEngine;

namespace Game.Core
{
    public class PlayerModel
    {
        public PhysicsBody Body { get; private set; }
        public PlayerConfig Config { get; private set; }

        public float MaxLaserCharge => 5f; // Максимум 5 секунд непрерывной работы
        public float LaserCharge { get; private set; } = 5f;
        public bool IsLaserActive { get; set; } // Флаг для синхронизации
        
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
        public void UpdateLaser(float dt)
        {
            if (IsLaserActive && LaserCharge > 0)
            {
                LaserCharge -= dt;
                if (LaserCharge <= 0) IsLaserActive = false;
            }
            else if (!IsLaserActive && LaserCharge < MaxLaserCharge)
            {
                // Восстанавливается в два раза медленнее, чем тратится
                LaserCharge += dt * 0.5f; 
            }
        }
        
        public void ConsumeLaser(float dt) => LaserCharge = Mathf.Max(0, LaserCharge - dt);
        public void RestoreLaser(float dt) => LaserCharge = Mathf.Min(MaxLaserCharge, LaserCharge + dt * 0.5f);
    }
}