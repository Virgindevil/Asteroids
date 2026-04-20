using System.Collections.Generic;
using Zenject;
using UnityEngine;
using Game.Core;

public class CollisionManager : ITickable
{
    // Списки мы получим через конструктор (ниже объясню как)
    private readonly PlayerModel _player;
    private readonly List<EnemyModel> _enemies;
    private readonly List<BulletModel> _projectiles;

    public CollisionManager(PlayerModel player, List<EnemyModel> enemies, List<BulletModel> projectiles)
    {
        _player = player;
        _enemies = enemies;
        _projectiles = projectiles;
    }

    public void Tick()
    {
        // 1. Проверка: Пули против Врагов
        for (int i = _projectiles.Count - 1; i >= 0; i--)
        {
            var bullet = _projectiles[i];
            for (int j = _enemies.Count - 1; j >= 0; j--)
            {
                var enemy = _enemies[j];
                if (CheckCollision(bullet, enemy))
                {
                    bullet.OnCollision(enemy);
                    enemy.OnCollision(bullet);
                }
            }
        }

        // 2. Проверка: Игрок против Врагов
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (CheckCollision(_player, _enemies[i]))
            {
                _player.OnCollision(_enemies[i]);
                _enemies[i].OnCollision(_player);
            }
        }
    }

    private bool CheckCollision(ICollidable a, ICollidable b)
    {
        // Используем твой Body.Position
        float distanceSqr = (a.Body.Position - b.Body.Position).sqrMagnitude;
        float radiusSum = a.CollisionRadius + b.CollisionRadius;
        return distanceSqr <= (radiusSum * radiusSum);
    }
}