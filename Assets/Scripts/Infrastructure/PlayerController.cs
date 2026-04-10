using Game.Core;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class PlayerController : ITickable, IFixedTickable
    {
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;
        private readonly ProjectilePool _pool;

        public PlayerController(PlayerModel model, IInputStrategy input, ProjectilePool pool)
        {
            _model = model;
            _input = input;
            _pool = pool;
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
        }

        // Фиксированный шаг: Физика движения
        public void FixedTick()
        {
            Vector2 moveDir = _input.GetMoveDirection();
            
            if (moveDir.sqrMagnitude > 0.01f)
            {
                // Применяем силу движения. 
                // Используем ускорение из конфига модели.
                float accel = _model.Config.MovementAcceleration;
                _model.Body.AddForce(moveDir * accel * Time.fixedDeltaTime);
            }
        }
        
        private void Shoot()
        {
            // Используем Forward из физики (он обновляется в Tick через Rotation)
            Vector2 shootDirection = _model.Body.Forward;
            Vector2 spawnPosition = _model.Body.Position + shootDirection * 0.5f;
            _pool.Spawn(spawnPosition, shootDirection);
        }
    }
}