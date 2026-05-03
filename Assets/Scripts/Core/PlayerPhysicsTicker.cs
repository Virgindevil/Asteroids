using UnityEngine;
using Zenject;

namespace  Game.Core
{
    public class PlayerPhysicsTicker : ITickable
    {
        private readonly PlayerModel _model;
        private readonly MapService _map;

        public PlayerPhysicsTicker(PlayerModel model, MapService map)
        {
            _model = model;
            _map = map;
        }

        public void Tick()
        {
            _model.Body.UpdatePhysics(Time.deltaTime);
            _model.Body.TeleportIfOutOfBounds(_map.Width, _map.Height);
        }
    }
}
