using UnityEngine;
using Zenject;

namespace  Game.Core
{
    public class PlayerPhysicsTicker : ITickable
    {
        private readonly PlayerModel _model;
        private readonly MapService _map;
        
        private readonly float _frictionMultiplier;
        private readonly float _teleportOffset;

        public PlayerPhysicsTicker(PlayerModel model, MapService map, WorldConfig worldConfig)
        {
            _model = model;
            _map = map;
            _frictionMultiplier = worldConfig.FrictionTimeMultiplier;
            _teleportOffset = worldConfig.TeleportBoundaryOffset;
        }

        public void Tick()
        {
            _model.Body.UpdatePhysics(Time.deltaTime, _frictionMultiplier);
            _model.Body.TeleportIfOutOfBounds(_map.Width+ _teleportOffset, _map.Height+ _teleportOffset);
        }
    }
}
