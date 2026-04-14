using Game.Core;
using UnityEngine;

public class AsteroidModel : EnemyModel
{
    public AsteroidModel(EnemyConfig config, Vector2 pos, Vector2 vel) 
        : base(config, pos, vel) { }

    public override void Update(float dt)
    {
        Body.UpdatePhysics(dt);
    }
}
