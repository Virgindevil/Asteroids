using Cysharp.Threading.Tasks;
using Game.Core;
using System;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerController : ITickable, IInitializable, IDisposable
    {
        private Camera _camera;
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;
        private readonly ProjectilePool _pool;
        private readonly SignalBus _signalBus;
        private bool _isProcessingLaser;

        public PlayerController(PlayerModel model, IInputStrategy input, ProjectilePool pool, 
            SignalBus signalBus)
        {
            _model = model;
            _input = input;
            _pool = pool;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _camera = Camera.main;
            _signalBus.Subscribe<PlayerRevivedSignal>(_model.Revive);
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }
        
        private void OnHealthChanged(PlayerHealthChangedSignal signal)
        {
            if (signal.CurrentHealth <= 0)
                _signalBus.Fire(new GameOverSignal());
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<PlayerRevivedSignal>(_model.Revive);
            _signalBus.TryUnsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        public void Tick()
        {
            if (Time.timeScale <= 0f) return;
            
            Vector2 moveDir = _input.GetMoveDirection();
            if (moveDir.sqrMagnitude > 0.01f && !_model.IsStunned)
                _model.Body.AddForce(moveDir * _model.Config.MovementAcceleration * Time.deltaTime);
            
            Vector2 playerScreenPos = _camera.WorldToScreenPoint(_model.Body.Position);
            Vector2 lookDir = _input.GetLookDirection(playerScreenPos);

            if (lookDir.sqrMagnitude > 0.01f)
                _model.Body.Rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

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