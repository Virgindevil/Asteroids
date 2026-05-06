using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerAimController : ITickable
    {
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;
        private readonly ICameraProvider _cameraProvider;

        public PlayerAimController(
            PlayerModel model,
            IInputStrategy input,
            ICameraProvider cameraProvider)
        {
            _model = model;
            _input = input;
            _cameraProvider = cameraProvider;
        }

        public void Tick()
        {
            if (Time.timeScale <= 0f) return;

            Vector2 playerScreenPos = _cameraProvider.Camera
                .WorldToScreenPoint(_model.Body.Position);
            Vector2 lookDir = _input.GetLookDirection(playerScreenPos);

            if (lookDir.sqrMagnitude > 0.01f)
                _model.Body.Rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        }
    }
}