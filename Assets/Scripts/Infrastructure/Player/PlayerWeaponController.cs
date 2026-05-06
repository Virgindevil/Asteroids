using System;
using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerWeaponController : ITickable
    {
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;
        private readonly ProjectilePool _pool;
        private readonly SignalBus _signalBus;
        private bool _isProcessingLaser;
    
        public PlayerWeaponController (PlayerModel model, IInputStrategy input, ProjectilePool pool, SignalBus signalBus)
        {
            _model = model;
            _input = input;
            _pool = pool;
            _signalBus = signalBus;
        }
        
        public void Tick()
        {
            if (_input.IsShooting())
                Shoot();

            _model.UpdateBulletCooldown(Time.deltaTime);
            HandleLaser();
        
        }
        private void Shoot()
        {
            if (_model.CanShoot)
            {
                Vector2 shootDirection = _model.Body.Forward;
                Vector2 spawnPosition = _model.Body.Position + shootDirection * _model.CollisionRadius;
                _pool.Spawn(spawnPosition, shootDirection);
                _model.SetShootCooldown();
            }
        }

        private void HandleLaser()
        {
            _model.UpdateLaser(Time.deltaTime);
            if (_input.IsLaserActive() && !_isProcessingLaser && _model.LaserCharge >= 1f)
                FireLaserPulse().Forget();
        }

        private async UniTask FireLaserPulse()
        {
            if (!_model.TryConsumeCharge()) return;
            
            _isProcessingLaser = true;
            _model.IsLaserActive = true;
            
            _signalBus.Fire(new LaserStateChangedSignal { IsActive = true });
            
            await UniTask.Delay(TimeSpan.FromSeconds(_model.Config.LaserActiveDuration),
                delayType: DelayType.DeltaTime);
            
            _model.IsLaserActive = false;
            
            _signalBus.Fire(new LaserStateChangedSignal { IsActive = false });
            
            await UniTask.Delay(TimeSpan.FromSeconds(_model.Config.LaserCooldownDelay),
                delayType: DelayType.DeltaTime);
            
            _isProcessingLaser = false;
        }
    } 
}

