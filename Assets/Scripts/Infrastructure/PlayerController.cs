using Game.Core;
using Zenject;
using UnityEngine;

namespace Game.Infrastructure
{
    public class PlayerController : IFixedTickable
    {
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;

        public PlayerController(PlayerModel model, IInputStrategy input)
        {
            _model = model;
            _input = input;
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
        }
    }
}