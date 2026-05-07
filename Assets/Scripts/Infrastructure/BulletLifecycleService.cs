using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class BulletLifecycleService : ITickable
    {
        private readonly float _frictionMultiplier;
        private readonly ProjectilePool _pool;
        private readonly IProjectileProvider _provider;

        private readonly List<BulletModel> _toRelease = new();

        public BulletLifecycleService(
            IProjectileProvider provider,
            ProjectilePool pool,
            WorldConfig worldConfig)
        {
            _provider = provider;
            _pool = pool;
            _frictionMultiplier = worldConfig.FrictionTimeMultiplier;
        }

        public void Tick()
        {
            var dt = Time.deltaTime;
            var bullets = _provider.ActiveProjectiles;
            
            for (var i = 0; i < bullets.Count; i++)
            {
                var b = bullets[i];
                if (!b.IsActive) continue;

                b.Body.UpdatePhysics(dt, _frictionMultiplier);
                b.LifeTime -= dt;

                if (b.LifeTime <= 0f)
                    b.IsActive = false;
            }
            
            for (var i = bullets.Count - 1; i >= 0; i--)
                if (!bullets[i].IsActive)
                    _toRelease.Add(bullets[i]);

            for (var i = 0; i < _toRelease.Count; i++)
                _pool.Release(_toRelease[i]);

            _toRelease.Clear();
        }
    }
}