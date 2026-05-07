using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerController : ITickable
    {
        private readonly IInputStrategy _input;
        private readonly PlayerModel _model;
        private readonly GameStateService _gameState;

        public PlayerController(PlayerModel model, IInputStrategy input,  GameStateService gameState)
        {
            _model = model;
            _input = input;
            _gameState = gameState;
        }

        public void Tick()
        {
            if (_gameState.IsPaused) return;

            var moveDir = _input.GetMoveDirection();
            if (moveDir.sqrMagnitude > 0.01f && !_model.IsStunned)
                _model.Body.AddForce(moveDir * _model.Config.MovementAcceleration * Time.deltaTime);
        }
    }
}