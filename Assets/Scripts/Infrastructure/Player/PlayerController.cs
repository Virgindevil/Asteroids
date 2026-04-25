using Cysharp.Threading.Tasks;
using Game.Core;
using System;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerController : ITickable, IFixedTickable
    {
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

        // Каждый кадр: Ввод и Поворот (для плавности визуализации)
        public void Tick()
        {
            // 1. Поворот: смотрим на мышь
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(_model.Body.Position);
            Vector2 lookDir = _input.GetLookDirection(playerScreenPos);
    
            if (lookDir.sqrMagnitude > 0.01f)
            {
                _model.Body.Rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            }

            // 2. Стрельба
            if (_input.IsShooting())
            {
                Shoot();
            }

            _model.UpdateBulletCooldown(Time.deltaTime);

            HandleLaser();
        }

        // Фиксированный шаг: Физика движения
        public void FixedTick()
        {
            Vector2 moveDir = _input.GetMoveDirection();
            
            if (moveDir.sqrMagnitude > 0.01f && !_model.IsStanned)
            {
                // Применяем силу движения. 
                // Используем ускорение из конфига модели.
                float accel = _model.Config.MovementAcceleration;
                _model.Body.AddForce(moveDir * accel * Time.fixedDeltaTime);
            }
        }
        
        private void Shoot()
        {
            if (_model.CanShoot)
            {
                Vector2 shootDirection = _model.Body.Forward;
                Vector2 spawnPosition = _model.Body.Position + shootDirection * 0.5f;
                _pool.Spawn(spawnPosition, shootDirection);
                _model.SetShootCooldown();
            }
            // Используем Forward из физики (он обновляется в Tick через Rotation)
            
        }

        private void HandleLaser()
        {
            _model.UpdateLaser(Time.deltaTime);

            // Если игрок хочет стрелять, мы не заняты прошлым выстрелом и есть заряд
            if (_input.IsLaserActive() && !_isProcessingLaser && _model.LaserCharge >= 1f)
            {
                FireLaserPulse().Forget();
            }
        }

        private async UniTaskVoid FireLaserPulse()
        {
            if (!_model.TryConsumeCharge()) return;

            _isProcessingLaser = true;
            _model.IsLaserActive = true;
            _signalBus.Fire(new LaserStateChangedSignal { IsActive = true });

            // Длительность одного выстрела лазера (например, 0.8 сек)
            // Можно тоже вынести в JSON как LaserDuration
            await UniTask.Delay(TimeSpan.FromSeconds(0.8f));

            _model.IsLaserActive = false;
            _signalBus.Fire(new LaserStateChangedSignal { IsActive = false });

            // Небольшая пауза между импульсами, чтобы лазер не сливался в одну линию
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            _isProcessingLaser = false;
        }
    }
}