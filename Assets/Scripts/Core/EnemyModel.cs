using Game.Core;
using UnityEngine;

public abstract class EnemyModel : ICollidable
{
    public PhysicsBody Body { get; }
    public EnemyConfig Config { get; }
    public float CollisionRadius => Config.CollisionRadius;

    protected EnemyModel(EnemyConfig config, Vector2 position, Vector2 velocity)
    {
        Config = config;
        Body = new PhysicsBody(position, config.Friction);
        Body.Velocity = velocity;
    }

    // У каждого врага может быть своя уникальная логика движения (Update)
    public abstract void Update(float dt);
    public abstract void OnCollision(ICollidable other);
}