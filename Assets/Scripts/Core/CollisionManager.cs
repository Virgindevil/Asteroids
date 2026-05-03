using Game.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollisionManager : ITickable, IInitializable, IDisposable
{
    private readonly PlayerModel _player;
    private readonly IEnemyProvider _enemyProvider;
    private readonly IProjectileProvider _projectileProvider;
    private readonly SignalBus _signalBus;

    private bool _isLaserActive;
    private readonly float _laserLength;

    public CollisionManager(
        PlayerModel player,
        IEnemyProvider enemyProvider,
        IProjectileProvider projectileProvider,
        SignalBus signalBus,
        PlayerConfig playerConfig)
    {
        _player = player;
        _enemyProvider = enemyProvider;
        _projectileProvider = projectileProvider;
        _signalBus = signalBus;
        _laserLength = playerConfig.LaserLength;
    }

    public void Initialize()
    {
        // Подписываемся на изменение состояния лазера
        _signalBus.Subscribe<LaserStateChangedSignal>(OnLaserStateChanged);
    }

    private void OnLaserStateChanged(LaserStateChangedSignal signal)
    {
        _isLaserActive = signal.IsActive;
    }

    public void Tick()
    {
        var enemies = _enemyProvider.ActiveEnemies;
        var projectiles = _projectileProvider.ActiveProjectiles;

        // 1. ЛАЗЕР vs ВРАГИ
        if (_isLaserActive)
        {
            CheckLaserCollisions(enemies);
        }

        // 2. ПУЛИ vs ВРАГИ
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

        // 3. ИГРОК vs ВРАГИ
        if (!_player.IsInvulnerable)
        {
            foreach (var enemy in enemies)
            {
                if (CheckCollision(_player, enemy))
                {
                    _player.OnCollision(enemy);
                    enemy.OnCollision(_player);
                }
            }
        }
    }

    private void CheckLaserCollisions(List<EnemyModel> enemies)
    {
        Vector2 origin = _player.Body.Position;
        
        // Считаем направление лазера на основе поворота игрока
        // В Unity 0 градусов — это обычно Right (ось X)
        float angleRad = _player.Body.Rotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Vector2 endPoint = origin + direction * _laserLength;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];
            
            // Проверяем, задевает ли отрезок (лазер) круг (врага)
            if (IsSegmentIntersectingCircle(origin, endPoint, enemy.Body.Position, enemy.CollisionRadius))
            {
                // Наносим большой урон для мгновенного уничтожения
                enemy.TakeDamage(999f);
                // OnCollision не вызываем, так как лазер — не физический объект ICollidable
            }
        }
    }

    // Геометрическая проверка: расстояние от точки до отрезка
    private bool IsSegmentIntersectingCircle(Vector2 start, Vector2 end, Vector2 center, float radius)
    {
        Vector2 line = end - start;
        Vector2 toCenter = center - start;
        float lineLenSq = line.sqrMagnitude;

        if (lineLenSq == 0) return (center - start).sqrMagnitude <= radius * radius;

        // Находим проекцию центра на прямую
        float t = Vector2.Dot(toCenter, line) / lineLenSq;
        // Ограничиваем t, чтобы оставаться в пределах отрезка
        t = Mathf.Clamp01(t);

        Vector2 closestPoint = start + t * line;
        float distSq = (center - closestPoint).sqrMagnitude;

        return distSq <= (radius * radius);
    }

    private bool CheckCollision(ICollidable a, ICollidable b)
    {
        float radiusSum = a.CollisionRadius + b.CollisionRadius;
        return (a.Body.Position - b.Body.Position).sqrMagnitude <= (radiusSum * radiusSum);
    }

    public void Dispose()
    {
        _signalBus.TryUnsubscribe<LaserStateChangedSignal>(OnLaserStateChanged);
    }
}