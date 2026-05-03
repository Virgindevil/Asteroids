using UnityEngine;

namespace Game.Core
{
    public class PlayerViewModel
    {
        private readonly PlayerModel _model;
        private readonly MapService _mapService;

        public PlayerViewModel(PlayerModel model, MapService mapService)
        {
            _model = model;
            _mapService = mapService;
        }

        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;
        public float RoundRotation => (_model.Body.Rotation % PhysicsBody.RoundDegree
                                       + PhysicsBody.RoundDegree) % PhysicsBody.RoundDegree;
        public float Speed => _model.Body.Velocity.magnitude;
        public int ReadyCharges => Mathf.FloorToInt(_model.LaserCharge);
        public float RechargeProgress => _model.LaserCharge % 1f;

        public float LaserLength => _model.Config.LaserLength;

        public float NextChargeCooldown
        {
            get
            {
                if (_model.LaserCharge >= _model.Config.MaxLaserCharges) return 0f;
                float partialCharge = _model.LaserCharge - Mathf.Floor(_model.LaserCharge);
                return (1f - partialCharge) * _model.Config.LaserCooldown;
            }
        }

        public string LaserStatusText
        {
            get
            {
                if (ReadyCharges >= _model.Config.MaxLaserCharges)
                    return $"LASER: {ReadyCharges} (READY)";
                float secondsLeft = (1f - RechargeProgress) * _model.Config.LaserCooldown;
                return $"LASER: {ReadyCharges} | NEXT: {secondsLeft:F1}s";
            }
        }

        public float GetChargeForSlider(int index) =>
            Mathf.Clamp01(_model.LaserCharge - index);
    }
}