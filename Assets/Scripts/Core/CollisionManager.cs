using Game.Core;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

    public class CollisionManager : ITickable
    {
        private readonly PlayerModel _player;
        private readonly IEnemyProvider _enemyProvider;
        private readonly IProjectileProvider _projectileProvider;

        public CollisionManager(PlayerModel player, IEnemyProvider enemyProvider, IProjectileProvider projectileProvider)
        {
            _player = player;
            _enemyProvider = enemyProvider;
            _projectileProvider = projectileProvider;
        }

        public void Tick()
        {
            var enemies = _enemyProvider.ActiveEnemies;
            var projectiles = _projectileProvider.ActiveProjectiles;

            // 1. ПУЛИ vs ВРАГИ
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
                        break;
                    }
                }
            }

            // 2. ИГРОК vs ВРАГИ
            if (!_player.IsInvulnerable)
            {
                foreach (var enemy in enemies)
                {
                    if (CheckCollision(_player, enemy))
                    {
                        _player.OnCollision(enemy);
                        enemy.OnCollision(_player);
                        PhysicsBody.ResolvePushApart(_player, enemy);
                    }
                }
            }
        }

        private bool CheckCollision(ICollidable a, ICollidable b)
        {
            float radiusSum = a.CollisionRadius + b.CollisionRadius;
            
            // Если участвует пуля, увеличиваем область в 2 раза, чтобы точно попадать
            //if (a is BulletModel || b is BulletModel) radiusSum *= 2f; 

            return (a.Body.Position - b.Body.Position).sqrMagnitude <= (radiusSum * radiusSum);
        }
    }
