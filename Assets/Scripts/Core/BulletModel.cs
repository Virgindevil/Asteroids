using UnityEngine;

namespace Game.Core
{
    public class BulletModel
    {
        public PhysicsBody Body { get; }
        public float LifeTime { get; private set; }
        public bool IsActive { get; set; }

        public BulletModel(Vector2 pos, Vector2 velocity)
        {
            // У пули нет трения (friction = 1.0), она не должна замедляться в космосе
            Body = new PhysicsBody(pos, 1.0f);
            Body.Velocity = velocity;
            LifeTime = 2.0f; // Пуля исчезает через 2 сек
            IsActive = true;
        }

        public void Update(float dt)
        {
            if (!IsActive) return;
            
            Body.UpdatePhysics(dt);
            LifeTime -= dt;
            
            if (LifeTime <= 0) IsActive = false;
        }
    }
}