using UnityEngine;

namespace Game.Core
{
    public class AsteroidViewModel
    {
        private readonly AsteroidModel _model;
        private readonly WorldConfig _worldConfig;

        public AsteroidViewModel(AsteroidModel model, WorldConfig worldConfig)
        {
            _model = model;
            _worldConfig = worldConfig;
        }

        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;

        public void Update(float dt)
        {
            _model.Update(dt);
            // Астероиды тоже должны телепортироваться при выходе за границы ?
            _model.Body.TeleportIfOutOfBounds(_worldConfig.Width, _worldConfig.Height);
        }
    }
}