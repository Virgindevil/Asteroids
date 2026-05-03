using UnityEngine;
using Game.Core;

namespace Game.Presentation
{
    public abstract class EnemyView : MonoBehaviour
    {
        protected EnemyModel Model;

        public void Initialize(EnemyModel model)
        {
            Model = model;
            float size = model.CollisionRadius * 2f;
            transform.localScale = new Vector3(size, size, 1f);
        }

        protected virtual void Update()
        {
            if (Model == null) return;
            transform.position = Model.Body.Position;
            transform.rotation = Quaternion.Euler(0, 0, Model.Body.Rotation);
        }
    }
}