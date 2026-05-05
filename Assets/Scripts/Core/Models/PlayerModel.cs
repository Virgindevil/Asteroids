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
        public float LaserCharge { get; private set; }
        public bool IsLaserActive { get; set; }
        public bool CanShoot => _shootTimer <= 0;

        public int Health { get; private set; }

        public float CollisionRadius => Config.CollisionRadius; 
        public bool IsInvulnerable { get; private set; }
        public bool IsStunned { get; private set; }

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
            Vector2 force = direction.normalized * Config.MovementAcceleration * deltaTime;
            Body.AddForce(force);
        }
        
        public void Revive()
        {
            Health = (int)Config.MaxHealth;
            IsStunned = false;
    
            _signalBus.Fire(new PlayerHealthChangedSignal { CurrentHealth = Health });
    
            SetInvulnerable(Config.InvulnerabilityDuration); 
        }
        
        public void SetInvulnerable(float duration)
        {
            RunInvulnerability(duration).Forget();
        }

        private async UniTaskVoid RunInvulnerability(float duration)
        {
            IsInvulnerable = true;
            IsStunned = true;
            _signalBus.Fire(new InvincibleEffectActiveSignal { IsActive = true });

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsInvulnerable = false;
            IsStunned = false;
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
            if (!IsLaserActive && LaserCharge < Config.MaxLaserCharges)
            {
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