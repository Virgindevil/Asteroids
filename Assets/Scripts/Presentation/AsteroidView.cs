using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Presentation
{
    public class AsteroidView : MonoBehaviour
    {
        private AsteroidModel _model;

        public void Initialize(AsteroidModel model)
        {
            _model = model;
            // Устанавливаем масштаб в зависимости от радиуса из конфига
            float size = _model.CollisionRadius * 2f;
            transform.localScale = new Vector3(size, size, 1f);
        }

        private void Update()
        {
            if (_model == null) return;

            // Синхронизация с кастомной физикой
            transform.position = _model.Body.Position;
            transform.rotation = Quaternion.Euler(0, 0, _model.Body.Rotation);
        }

        public class Factory : PlaceholderFactory<AsteroidView> { }
    }
}