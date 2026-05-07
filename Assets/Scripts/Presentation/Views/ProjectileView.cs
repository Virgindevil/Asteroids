using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class ProjectileView : MonoBehaviour
    {
        private BulletModel _model;

        private void Update()
        {
            transform.position = _model.Body.Position;
        }

        public void Initialize(BulletModel model)
        {
            _model = model;
            transform.position = _model.Body.Position;
        }

        public class Factory : PlaceholderFactory<ProjectileView>
        {
        }
    }
}