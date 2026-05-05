using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class PhysicsBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; } 
        public float AngularVelocity { get; set; } 
        
        public float Speed => Velocity.magnitude;
        public Vector2 Forward => new Vector2(
            Mathf.Cos((Rotation) * Mathf.Deg2Rad),
            Mathf.Sin((Rotation) * Mathf.Deg2Rad)
            ).normalized;

        public const int RoundDegree = 360;
        public const float _frictionTimeMultiplier = 10f;

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
            Velocity *= Mathf.Pow(_friction, deltaTime * _frictionTimeMultiplier);
            Position += Velocity * deltaTime;

            AngularVelocity *= Mathf.Pow(_angularFriction, deltaTime * _frictionTimeMultiplier);
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

        public void ResetState(Vector2 newPos, Vector2 newVel = default)
        {
            Position = newPos;
            Velocity = newVel;
            AngularVelocity = 0f;
        }

        public static void ResolvePushApart(ICollidable a, ICollidable b, float bounce = 0.8f)
        {
            Vector2 diff = a.Body.Position - b.Body.Position;
            float distance = diff.magnitude;

            if (distance < 0.0001f)
            {
                a.Body.Position += new Vector2(0.02f, 0);
                return;
            }

            Vector2 normal = diff / distance;

            Vector2 relativeVelocity = a.Body.Velocity - b.Body.Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            if (velocityAlongNormal < 0)
            {
                float j = -(1 + bounce) * velocityAlongNormal;
                j /= 2; 

                Vector2 impulse = j * normal;
                a.Body.Velocity += impulse;
                b.Body.Velocity -= impulse;
            }

            float overlap = (a.CollisionRadius + b.CollisionRadius) - distance;
            if (overlap > 0)
            {
                const float percent = 0.5f; 
                Vector2 correction = normal * (overlap * percent);
                a.Body.Position += correction;
                b.Body.Position -= correction;
            }
        }
    }
}