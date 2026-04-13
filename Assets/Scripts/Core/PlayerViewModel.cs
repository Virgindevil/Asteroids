using UnityEngine;

namespace Game.Core
{
    public class PlayerViewModel
    {
        private readonly PlayerModel _model;
        private readonly WorldConfig _worldConfig;


        public PlayerViewModel(PlayerModel model, WorldConfig worldConfig)
        {
            _model = model;
            _worldConfig = worldConfig;
        }

        // Пробрасываем данные для отображения
        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;
        public float RoundRotation => (_model.Body.Rotation % PhysicsBody.RoundDegree + PhysicsBody.RoundDegree) % PhysicsBody.RoundDegree;
        public float Speed => _model.Body.Velocity.magnitude;
        public bool IsLaserActive => _model.IsLaserActive;

        public void Update(float deltaTime)
        {
            // Безопасная проверка: если чего-то нет, просто не считаем физику в этом кадре
            if (_model?.Body == null || _worldConfig == null) return;

            _model.Body.UpdatePhysics(deltaTime);
            _model.Body.TeleportIfOutOfBounds(_worldConfig.Width, _worldConfig.Height);
        }
    }
}