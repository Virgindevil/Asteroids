using UnityEngine; // Используем только Vector2, никакой логики MonoBehaviour!

namespace Game.Core
{
    public class PhysicsBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; } // В градусах

        private float _friction;

        public PhysicsBody(Vector2 startPos, float friction)
        {
            Position = startPos;
            _friction = friction;
        }

        public void AddForce(Vector2 force)
        {
            Velocity += force;
        }

        public void UpdatePhysics(float deltaTime)
        {
            // Применяем трение (инерция постепенно гаснет)
            Velocity *= Mathf.Pow(_friction, deltaTime * 10f);

            // Обновляем позицию
            Position += Velocity * deltaTime;
        }

        // Логика портала (Screen Wrap)
        public void TeleportIfOutOfBounds(float width, float height)
        {
            float halfW = width / 2f;
            float halfH = height / 2f;

            Vector2 pos = Position;

            if (pos.x > halfW) pos.x = -halfW;
            else if (pos.x < -halfW) pos.x = halfW;

            if (pos.y > halfH) pos.y = -halfH;
            else if (pos.y < -halfH) pos.y = halfH;

            Position = pos;
        }
    }
}