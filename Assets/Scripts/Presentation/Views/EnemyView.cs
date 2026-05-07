using Game.Core;
using UnityEngine;

namespace Game.Presentation
{
    public abstract class EnemyView : MonoBehaviour
    {
        protected EnemyModel Model;

        protected virtual void Update()
        {
            if (Model == null) return;
            transform.position = Model.Body.Position;
            transform.rotation = Quaternion.Euler(0, 0, Model.Body.Rotation);
        }

        public void Initialize(EnemyModel model)
        {
            Model = model;
            var size = model.CollisionRadius * 2f;
            transform.localScale = new Vector3(size, size, 1f);
        }
    }
}