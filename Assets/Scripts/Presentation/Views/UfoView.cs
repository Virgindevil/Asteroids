using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Presentation
{
    public class UfoView : EnemyView
    {
        [Inject]
        public void Construct(Vector2 startPosition)
        {
            transform.position = startPosition;
        }

        // Initialize и Update наследуются из EnemyView

        public class Factory : PlaceholderFactory<Vector2, UfoView> { }
    }
}