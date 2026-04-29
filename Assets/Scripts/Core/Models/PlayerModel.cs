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

            // ВОТ ЭТОЙ СТРОКИ НЕ ХВАТАЛО:
            _signalBus.Subscribe<PlayerRevivedSignal>(Revive);
        }
        
        public void Accelerate(Vector2 direction, float deltaTime)
        {
            // Сила = Направление * Ускорение * Время
            Vector2 force = direction.normalized * Config.MovementAcceleration * deltaTime;
            Body.AddForce(force);
        }
        
        // Метод OnRevived удаляем полностью, оставляем только этот:
        private void Revive()
        {
            Health = (int)Config.MaxHealth;
            IsStanned = false; // На всякий случай снимаем стан, если он завис
    
            // Оповещаем UI
            _signalBus.Fire(new PlayerHealthChangedSignal { CurrentHealth = Health });
    
            SetInvulnerable(3f); // Даем неуязвимость после возрождения
            Debug.Log("[PlayerModel] Здоровье восстановлено после возрождения");
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
            if (IsInvulnerable || Health <= 0) 
                return;

            if (other is EnemyModel enemy)
            {
                // Отталкивание от врага
                // Формула отражения вектора: v = v - 2 * (v ⋅ n) * n
                Vector2 normal = (Body.Position - other.Body.Position).normalized;
                Body.Velocity = Vector2.Reflect(Body.Velocity, normal) * 0.8f; // 0.8 - потеря энергии
                enemy.Body.Velocity -= normal * 1.5f;

                Debug.Log($"[Player] Collision with ENEMY: {enemy.Config.EnemyType}");

                // Запуск неуязвимости (как в оригинале)
                TakeDamage(1);
                if (Health > 0)
                {
                    SetInvulnerable(Config.InvulnerabilityDuration);
                }
            }
        }
        
        public void TakeDamage(int amount)
        {
            if (IsInvulnerable || Health <= 0) return;

            Health -= amount;

            _signalBus.Fire(new PlayerHealthChangedSignal { CurrentHealth = Health });

            if (Health <= 0)
            {
                IsInvulnerable = false;
                // Игрок умер
                _signalBus.Fire(new GameOverSignal());
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