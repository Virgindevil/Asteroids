using UnityEngine;

namespace Game.Core
{
    public class PhysicsBody
    {
        public const int RoundDegree = 360;
        private readonly float _angularFriction;

        private readonly float _friction;


        public PhysicsBody(Vector2 startPos, float friction = 0.95f, float angularFriction = 0.9f)
        {
            Position = startPos;
            _friction = friction;
            _angularFriction = angularFriction;
        }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }

        public float Speed => Velocity.magnitude;

        public Vector2 Forward => new Vector2(
            Mathf.Cos(Rotation * Mathf.Deg2Rad),
            Mathf.Sin(Rotation * Mathf.Deg2Rad)
        ).normalized;

        public void AddForce(Vector2 force)
        {
            Velocity += force;
        }

        public void AddTorque(float torque)
        {
            AngularVelocity += torque;
        }

        public void UpdatePhysics(float deltaTime, float frictionMultiplier)
        {
            Velocity *= Mathf.Pow(_friction, deltaTime * frictionMultiplier);
            Position += Velocity * deltaTime;

            AngularVelocity *= Mathf.Pow(_angularFriction, deltaTime * frictionMultiplier);
            Rotation = Mathf.Repeat(Rotation + AngularVelocity * deltaTime, RoundDegree);
        }

        public void TeleportIfOutOfBounds(float width, float height, float radius = 0f)
        {
            var halfW = width / 2f + radius;
            var halfH = height / 2f + radius;

            var pos = Position;

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
            var diff = a.Body.Position - b.Body.Position;
            var distance = diff.magnitude;

            if (distance < 0.0001f)
            {
                a.Body.Position += new Vector2(0.02f, 0);
                return;
            }

            var normal = diff / distance;

            var relativeVelocity = a.Body.Velocity - b.Body.Velocity;
            var velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            if (velocityAlongNormal < 0)
            {
                var j = -(1 + bounce) * velocityAlongNormal;
                j /= 2;

                var impulse = j * normal;
                a.Body.Velocity += impulse;
                b.Body.Velocity -= impulse;
            }

            var overlap = a.CollisionRadius + b.CollisionRadius - distance;
            if (overlap > 0)
            {
                const float percent = 0.5f;
                var correction = normal * (overlap * percent);
                a.Body.Position += correction;
                b.Body.Position -= correction;
            }
        }
    }
}