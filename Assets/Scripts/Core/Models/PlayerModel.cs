using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.Core
{
    public class PlayerModel : ICollidable
    {
        public PhysicsBody Body { get; private set; }
        public PlayerConfig Config { get; private set; }

        // ТЕПЕРЬ ОПЕРИРУЕМ ЗАРЯДАМИ
        public float LaserCharge { get; private set; } // От 0 до MaxLaserCharges
        public bool IsLaserActive { get; set; }

        public int Health { get; private set; }

        public float CollisionRadius => 0.5f; // Можно вынести в конфиг
        public bool IsInvulnerable { get; private set; }

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

        public void SetInvulnerable(float duration)
        {
            RunInvulnerability(duration).Forget();
        }

        private async UniTaskVoid RunInvulnerability(float duration)
        {
            IsInvulnerable = true;
            // Здесь можно кинуть сигнал для включения Particle System (кольца)
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            IsInvulnerable = false;
        }

        public void OnCollision(ICollidable other)
        {
            if (IsInvulnerable) 
                return;

            if (other is EnemyModel enemy)
            {
                // Отталкивание от врага
                Vector2 normal = (Body.Position - enemy.Body.Position).normalized;
                Body.Velocity += normal * 3f; // Игрок отталкивается сильнее
                enemy.Body.Velocity -= normal * 1.5f;

                Debug.Log($"[Player] Collision with ENEMY: {enemy.Config.EnemyType}");

                // Запуск неуязвимости (как в оригинале)
                SetInvulnerable(3f);
            }
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