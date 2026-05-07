// Infrastructure/BulletLifecycleService.cs
using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class BulletLifecycleService : ITickable
    {
        private readonly IProjectileProvider _provider;
        private readonly ProjectilePool _pool;
        private readonly float _frictionMultiplier;

        // Список пуль к уничтожению — переиспользуем чтобы не аллоцировать каждый кадр
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
            float dt = Time.deltaTime;
            var bullets = _provider.ActiveProjectiles;

            // 1. Обновляем физику и время жизни всех активных пуль
            for (int i = 0; i < bullets.Count; i++)
            {
                var b = bullets[i];
                if (!b.IsActive) continue;

                b.Body.UpdatePhysics(dt, _frictionMultiplier);
                b.LifeTime -= dt;

                if (b.LifeTime <= 0f)
                    b.IsActive = false;
            }

            // 2. Собираем мёртвые пули — итерируем с конца чтобы не сбить индексы
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (!bullets[i].IsActive)
                    _toRelease.Add(bullets[i]);
            }

            // 3. Освобождаем после итерации — Release файрит BulletDestroyedSignal,
            //    ProjectileManager услышит сигнал и уничтожит View
            for (int i = 0; i < _toRelease.Count; i++)
                _pool.Release(_toRelease[i]);

            _toRelease.Clear();
        }
    }
}