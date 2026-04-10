using Game.Core;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class PlayerController : IFixedTickable
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

        public void FixedTick()
        {
            float dt = Time.fixedDeltaTime;

            if (_input.IsAccelerating())
            {
                _model.Accelerate(_input.GetRotationDirection(_model.Body.Position), dt);
            }

            // Поворот за направлением ввода
            Vector2 dir = _input.GetRotationDirection(_model.Body.Position);
            if (dir.sqrMagnitude > 0.01f)
            {
                _model.Body.Rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            }
            
            if (_input.IsShooting())
            {
                Shoot();
            }
        }
        
        private void Shoot()
        {
            // Пуля летит туда, куда смотрит игрок
            Vector2 shootDirection = _model.Body.Forward;
            Vector2 spawnPosition = _model.Body.Position + shootDirection * 0.5f; // Спавним чуть впереди
            
            _pool.Spawn(spawnPosition, shootDirection);
        }
        
    }
}