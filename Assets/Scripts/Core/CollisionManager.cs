using System;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class CollisionManager : ITickable, IInitializable, IDisposable
{
    private readonly IEnemyProvider _enemyProvider;
    private readonly float _laserDamage;
    private readonly float _laserLength;
    private readonly PlayerModel _player;
    private readonly IProjectileProvider _projectileProvider;
    private readonly SignalBus _signalBus;

    private bool _isLaserActive;

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
        _laserDamage = playerConfig.LaserDamage;
    }

    public void Dispose()
    {
        _signalBus.TryUnsubscribe<LaserStateChangedSignal>(OnLaserStateChanged);
    }

    public void Initialize()
    {
        _signalBus.Subscribe<LaserStateChangedSignal>(OnLaserStateChanged);
    }

    public void Tick()
    {
        var enemies = _enemyProvider.ActiveEnemies;
        var projectiles = _projectileProvider.ActiveProjectiles;

        if (_isLaserActive) CheckLaserCollisions(enemies);

        for (var i = projectiles.Count - 1; i >= 0; i--)
        {
            var bullet = projectiles[i];
            if (!bullet.IsActive) continue;

            for (var j = enemies.Count - 1; j >= 0; j--)
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

        if (!_player.IsInvulnerable)
            foreach (var enemy in enemies)
                if (CheckCollision(_player, enemy))
                {
                    _player.OnCollision(enemy);
                    enemy.OnCollision(_player);
                }
    }

    private void OnLaserStateChanged(LaserStateChangedSignal signal)
    {
        _isLaserActive = signal.IsActive;
    }

    private void CheckLaserCollisions(IReadOnlyList<EnemyModel> enemies)
    {
        var origin = _player.Body.Position;

        var angleRad = _player.Body.Rotation * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        var endPoint = origin + direction * _laserLength;

        for (var i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];

            if (IsSegmentIntersectingCircle(origin, endPoint, enemy.Body.Position, enemy.CollisionRadius))
                enemy.TakeDamage(_laserDamage);
        }
    }

    private bool IsSegmentIntersectingCircle(Vector2 start, Vector2 end, Vector2 center, float radius)
    {
        var line = end - start;
        var toCenter = center - start;
        var lineLenSq = line.sqrMagnitude;

        if (lineLenSq == 0)
            return (center - start).sqrMagnitude <= radius * radius;

        var t = Vector2.Dot(toCenter, line) / lineLenSq;
        t = Mathf.Clamp01(t);

        var closestPoint = start + t * line;
        var distSq = (center - closestPoint).sqrMagnitude;

        return distSq <= radius * radius;
    }

    private bool CheckCollision(ICollidable a, ICollidable b)
    {
        var radiusSum = a.CollisionRadius + b.CollisionRadius;
        return (a.Body.Position - b.Body.Position).sqrMagnitude <= radiusSum * radiusSum;
    }
}
}
