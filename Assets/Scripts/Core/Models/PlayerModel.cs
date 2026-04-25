using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class PlayerModel : ICollidable
    {
        public PhysicsBody Body { get; private set; }
        public PlayerConfig Config { get; private set; }
        
        private readonly SignalBus _signalBus;

        private float _shootTimer;
        public float LaserCharge { get; private set; } // От 0 до MaxLaserCharges
        public bool IsLaserActive { get; set; }
        public bool CanShoot => _shootTimer <= 0;

        public int Health { get; private set; }

        public float CollisionRadius => 0.5f; // Можно вынести в конфиг
        public bool IsInvulnerable { get; private set; }
        public bool IsStanned { get; private set; }

        public PlayerModel(PlayerConfig config, SignalBus signalBus)
        {
            Config = config;
            _signalBus = signalBus;
            Health = (int)config.MaxHealth;
            Body = new PhysicsBody(Vector2.zero, config.Friction);

            _shootTimer = 0;
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
            IsStanned = true;
            //Debug.Log("<color=green>[PlayerModel]</color> Fire Invincible SIGNAL: TRUE");
            _signalBus.Fire(new InvincibleEffectActiveSignal { IsActive = true });

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsInvulnerable = false;
            IsStanned = false;
            //Debug.Log("<color=red>[PlayerModel]</color> Fire Invincible SIGNAL: FALSE");
            _signalBus.Fire(new InvincibleEffectActiveSignal { IsActive = false });
        }

        public void SetShootCooldown()
        {
            _shootTimer = Config.ShootCooldown;
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
                TakeDamage(1);
                SetInvulnerable(3f);
            }
        }
        
        public void TakeDamage(int amount)
        {
            if (IsInvulnerable) return;

            Health -= amount;
            if (Health < 0) Health = 0;

            // Отправляем сигнал об изменении здоровья
            _signalBus.Fire(new PlayerHealthChangedSignal { CurrentHealth = Health });

            if (Health > 0)
            {
                SetInvulnerable(Config.InvulnerabilityDuration); // Берем длительность из конфига
            }
        }

        public void UpdateBulletCooldown(float dt)
        {
            if (!CanShoot)
            {
                _shootTimer -= dt;
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