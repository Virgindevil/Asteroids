using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerAimController : ITickable
    {
        private readonly ICameraProvider _cameraProvider;
        private readonly IInputStrategy _input;
        private readonly PlayerModel _model;
        private readonly GameStateService _gameState;

        public PlayerAimController(
            PlayerModel model,
            IInputStrategy input,
            ICameraProvider cameraProvider,
            GameStateService gameState)
        {
            _model = model;
            _input = input;
            _cameraProvider = cameraProvider;
            _gameState = gameState;
        }

        public void Tick()
        {
            if (_gameState.IsPaused) return;

            var playerScreenPos = _cameraProvider.WorldToScreenPoint(_model.Body.Position);
            var lookDir = _input.GetLookDirection(playerScreenPos);

            if (lookDir.sqrMagnitude > 0.01f)
                _model.Body.Rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        }
    }
}