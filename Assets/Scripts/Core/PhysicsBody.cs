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
            Mathf.Cos(Rotation * Mathf.Deg2Rad),
            Mathf.Sin(Rotation * Mathf.Deg2Rad)
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
            float halfW = width / 2f - radius;
            float halfH = height / 2f - radius;

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

        public static void ResolvePushApart(ICollidable a, ICollidable b)
        {
            Vector2 diff = a.Body.Position - b.Body.Position;
            float distance = diff.magnitude;
            if (distance < 0.0001f) return; // Защита от деления на 0

            Vector2 normal = diff / distance;

            // 1. Смена скоростей (отскок)
            float impulse = 2f; 
            a.Body.Velocity += normal * impulse;
            b.Body.Velocity -= normal * impulse;

            // 2. Мягкое раздвижение (Penetration Recovery)
            // Это предотвратит лаги из-за того, что объекты застряли внутри друг друга
            float overlap = (a.CollisionRadius + b.CollisionRadius) - distance;
            if (overlap > 0)
            {
                a.Body.Position += normal * (overlap * 0.5f);
                b.Body.Position -= normal * (overlap * 0.5f);
            }
        }
    }
}