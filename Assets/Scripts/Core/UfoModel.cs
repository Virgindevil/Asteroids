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
        // Логика НЛО: летим не просто прямо, а немного доворачиваем к игроку
        Vector2 directionToPlayer = (_player.Body.Position - Body.Position).normalized;
        Body.Velocity = Vector2.Lerp(Body.Velocity, directionToPlayer * Config.Speed, dt * 0.5f);

        Body.UpdatePhysics(dt);
    }

    public override void OnCollision(ICollidable other) { /* Взрыв */ }
}