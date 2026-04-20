using Game.Core;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollisionManager : ITickable
{
    private readonly PlayerModel _player;
    private readonly IEnemyProvider _enemyProvider;
    private readonly IProjectileProvider _projectileProvider;

    // Конструктор принимает провайдеры (интерфейсы из Reesters.cs)
    public CollisionManager(
        PlayerModel player, 
        IEnemyProvider enemyProvider, 
        IProjectileProvider projectileProvider)
    {
        _player = player;
        _enemyProvider = enemyProvider;
        _projectileProvider = projectileProvider;
        Debug.Log("<color=green>[CollisionManager] Created!</color>");
    }

    public void Tick()
    {
        var eCount = _enemyProvider.ActiveEnemies?.Count ?? -1;
        var pCount = _projectileProvider.ActiveProjectiles?.Count ?? -1;
    
        if (Time.frameCount % 60 == 0) // Логаем раз в секунду, чтобы не спамить
        {
            Debug.Log($"[CollisionManager] Tick. Enemies: {eCount}, Projectiles: {pCount}");
        }
        
        // Если этот лог не появится — значит NonLazy() в инсталлере не сработал
        //Debug.Log("Collision Tick"); 

        var enemies = _enemyProvider.ActiveEnemies;
        var projectiles = _projectileProvider.ActiveProjectiles;

        // 1. Пули vs Враги
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            var bullet = projectiles[i];
            if (!bullet.IsActive) continue;

            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                var enemy = enemies[j];
                if (CheckCollision(bullet, enemy))
                {
                    bullet.OnCollision(enemy);
                    enemy.OnCollision(bullet);
                }
            }
        }

        // 2. Игрок vs Враги
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (CheckCollision(_player, enemies[i]))
            {
                _player.OnCollision(enemies[i]);
                enemies[i].OnCollision(_player);
            }
        }
    }

    private bool CheckCollision(ICollidable a, ICollidable b)
    {
        float distanceSqr = (a.Body.Position - b.Body.Position).sqrMagnitude;
        float radiusSum = a.CollisionRadius + b.CollisionRadius;
        return distanceSqr <= (radiusSum * radiusSum);
    }
}