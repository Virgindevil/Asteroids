using UnityEngine;

namespace Game.Core
{
    public class PhysicsBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; } // В градусах [0, 360)
        public float AngularVelocity { get; set; } // В градусах/сек
        
        public float Speed => Velocity.magnitude;
        public Vector2 Forward => new Vector2(
    Mathf.Cos((Rotation) * Mathf.Deg2Rad),
    Mathf.Sin((Rotation) * Mathf.Deg2Rad)
).normalized;

        public const int RoundDegree = 360;
        private const float _timeMultiplayer = 10f;

        private readonly float _friction;
        private readonly float _angularFriction;

        public PhysicsBody(Vector2 startPos, float friction = 0.95f, float angularFriction = 0.9f)
        {
            Position = startPos;
            _friction = friction;
            _angularFriction = angularFriction;
        }

        public void AddForce(Vector2 force) => Velocity += force;
        public void AddTorque(float torque) => AngularVelocity += torque;

        public void UpdatePhysics(float deltaTime)
        {
            // Линейное движение с трением
            Velocity *= Mathf.Pow(_friction, deltaTime * _timeMultiplayer);
            Position += Velocity * deltaTime;

            // Вращение с трением
            AngularVelocity *= Mathf.Pow(_angularFriction, deltaTime * _timeMultiplayer);
            Rotation = Mathf.Repeat(Rotation + AngularVelocity * deltaTime, RoundDegree);
        }

        public void TeleportIfOutOfBounds(float width, float height, float radius = 0f)
        {
            float halfW = width / 2f + radius;
            float halfH = height / 2f + radius;

            Vector2 pos = Position;

            if (pos.x > halfW) 
                pos.x = -halfW;
            else if (pos.x < -halfW) 
                pos.x = halfW;

            if (pos.y > halfH) 
                pos.y = -halfH;
            else if (pos.y < -halfH) 
                pos.y = halfH;

            Position = pos;
        }

        public static bool CheckCircleCollision(
            Vector2 a, float radiusA,
            Vector2 b, float radiusB)
        {
            float distSqr = (b - a).sqrMagnitude;
            float minDistSqr = (radiusA + radiusB) * (radiusA + radiusB);
            return distSqr <= minDistSqr;
        }

        public void ReflectVelocity(Vector2 normal, float bounceMultiplier = 0.8f)
        {
            Velocity = Vector2.Reflect(Velocity, normal) * bounceMultiplier;
        }

        public void ResetState(Vector2 newPos, Vector2 newVel = default)
        {
            Position = newPos;
            Velocity = newVel;
            AngularVelocity = 0f;
        }

        public static void ResolvePushApart(ICollidable a, ICollidable b, float bounce = 0.8f)
        {
            // 1. Считаем вектор между объектами
            Vector2 diff = a.Body.Position - b.Body.Position;
            float distance = diff.magnitude;

            // Защита от деления на ноль, если объекты в одной точке
            if (distance < 0.0001f)
            {
                a.Body.Position += new Vector2(0.01f, 0); // Чуть-чуть расталкиваем
                return;
            }

            Vector2 normal = diff / distance;

            // --- ЛОГИКА ОТСКОКА (Reflect) ---
            // Находим относительную скорость (как быстро они сближаются)
            Vector2 relativeVelocity = a.Body.Velocity - b.Body.Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            // Если объекты уже разлетаются в разные стороны, импульс не нужен
            if (velocityAlongNormal < 0)
            {
                // Вычисляем силу импульса (упрощенная модель без учета массы)
                float j = -(1 + bounce) * velocityAlongNormal;
                j /= 2; // Делим на 2, так как распределяем силу между двумя телами

                Vector2 impulse = j * normal;
                a.Body.Velocity += impulse;
                b.Body.Velocity -= impulse;
            }

            // --- ЛОГИКА РАЗДВИЖЕНИЯ (Penetration Recovery) ---
            float overlap = (a.CollisionRadius + b.CollisionRadius) - distance;
            if (overlap > 0)
            {
                // Мягко разводим объекты, чтобы они не дрожали и не застревали
                const float percent = 0.5f; // На сколько сильно выталкивать за один кадр
                Vector2 correction = normal * (overlap * percent);
                a.Body.Position += correction;
                b.Body.Position -= correction;
            }
        }

    }
}