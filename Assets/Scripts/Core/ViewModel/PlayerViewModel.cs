using UnityEngine;

namespace Game.Core
{
    public class PlayerViewModel
    {
        private readonly PlayerModel _model;

        public PlayerViewModel(PlayerModel model)
        {
            _model = model;
        }

        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;
        public float RoundRotation => (_model.Body.Rotation % PhysicsBody.RoundDegree
                                       + PhysicsBody.RoundDegree) % PhysicsBody.RoundDegree;
        public float Speed => _model.Body.Velocity.magnitude;

        public float LaserLength => _model.Config.LaserLength;

        public float GetChargeForSlider(int index) =>
            Mathf.Clamp01(_model.LaserCharge - index);
    }
}