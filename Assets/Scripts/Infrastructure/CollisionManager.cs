using Game.Core;
using UnityEngine;
using Zenject;

public class CollisionManager : ITickable
{
    private readonly PlayerModel _player;
    // Сюда мы потом добавим списки астероидов и пуль

    public CollisionManager(PlayerModel player)
    {
        _player = player;
    }

    public void Tick()
    {
        // Пока проверим только игрока (в будущем тут будет цикл по всем объектам)
        CheckPlayerCollisions();
    }

    private void CheckPlayerCollisions()
    {
        if (_player.IsInvulnerable) 
            return;

        //Пример логики:
        /* foreach (var enemy in _enemyPool.ActiveEnemies)
        {
            if (PhysicsBody.CheckCircleCollision(_player.Body.Position, _player.CollisionRadius,
                                               enemy.Position, enemy.Radius))
            {
                ResolveCollision(_player, enemy);
            }
        }*/
    }

    private void ResolveCollision(ICollidable a, ICollidable b)
    {
        // Математика отскока (Рикошет)
        Vector2 normal = (a.Body.Position - b.Body.Position).normalized;

        // Отражаем вектора скоростей
        a.Body.ReflectVelocity(normal);
        b.Body.ReflectVelocity(-normal);

        a.OnCollision(b);
        b.OnCollision(a);
    }
}