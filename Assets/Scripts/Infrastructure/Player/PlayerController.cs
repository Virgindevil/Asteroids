using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerController : ITickable
    {
        private readonly PlayerModel _model;
        private readonly IInputStrategy _input;
        private bool _isProcessingLaser;

        public PlayerController(PlayerModel model, IInputStrategy input)
        {
            _model = model;
            _input = input;
        }

        public void Tick()
        {
            if (Time.timeScale <= 0f) return;
            
            Vector2 moveDir = _input.GetMoveDirection();
            if (moveDir.sqrMagnitude > 0.01f && !_model.IsStunned)
                _model.Body.AddForce(moveDir * _model.Config.MovementAcceleration * Time.deltaTime);
        }
    }
}