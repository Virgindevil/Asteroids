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
        // 1. Движение к игроку (плавный доворот вектора скорости)
        Vector2 directionToPlayer = (_player.Body.Position - Body.Position).normalized;
        Body.Velocity = Vector2.Lerp(Body.Velocity, directionToPlayer * Config.Speed, dt * 0.5f);

        // 2. Поворот на игрока
        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            // Вычисляем целевой угол в градусах
            float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            // Если НЛО должно смотреть «носом» спрайта, и он нарисован смотрящим вверх:
            targetAngle -= 90f; 

            // Плавно поворачиваем (LerpAngle правильно обрабатывает переход через 360/0)
            Body.Rotation = Mathf.LerpAngle(Body.Rotation, targetAngle, dt * 2f);
        }

        // 3. Обсчет физики (позиция и трение)
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