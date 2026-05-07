using UnityEngine;
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

        public class Factory : PlaceholderFactory<Vector2, UfoView>
        {
        }
    }
}