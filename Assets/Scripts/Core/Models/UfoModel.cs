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

    public override void OnCollision(ICollidable other)
    {
        // ОБЯЗАТЕЛЬНО вызываем базовый метод, чтобы работало TakeDamage и логи в консоли
        base.OnCollision(other);

        // Если нужно добавить специфичное поведение только для НЛО при столкновении,
        // пиши его ниже:
        if (other is PlayerModel)
        {
            // Например, НЛО может отлетать сильнее при таране игрока
            Debug.Log("[UFO] Collided with Player!");
        }
    }
}