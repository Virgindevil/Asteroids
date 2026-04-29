using UnityEngine;

namespace Game.Core
{
    public class AsteroidViewModel
    {
        private readonly EnemyModel _model;
        private readonly MapService _mapService;

        public AsteroidViewModel(EnemyModel model, MapService mapService)
        {
            _model = model;
            _mapService = mapService;
        }

        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;

        public void Update(float dt)
        {
            _model.Update(dt);
            // Астероиды тоже должны телепортироваться при выходе за границы ?
            _model.Body.TeleportIfOutOfBounds(_mapService.Width, _mapService.Height);
        }
    }
}