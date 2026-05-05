using Game.Core;
using UnityEngine;

public class UfoModel : EnemyModel
{
    private readonly PlayerModel _player;

    public UfoModel(EnemyConfig config, Vector2 pos, Vector2 vel, PlayerModel player)
        : base(config, pos, vel)
    {
        _player = player;
    }

    public override void Update(float dt)
    {
        Vector2 directionToPlayer = (_player.Body.Position - Body.Position).normalized;
        Body.Velocity = Vector2.Lerp(Body.Velocity, directionToPlayer * Config.Speed, dt * 0.5f);

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            targetAngle -= 90f; 
            Body.Rotation = Mathf.LerpAngle(Body.Rotation, targetAngle, dt * 2f);
        }

        Body.UpdatePhysics(dt);
    }
}