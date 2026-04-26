using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class ProjectileManager : IInitializable, IDisposable, IProjectileProvider, ITickable
    {
        private readonly SignalBus _signalBus;
        private readonly ProjectileView.Factory _factory;
        private readonly PlayerConfig _playerConfig;
        private readonly ProjectilePool _projectilePool;

        private readonly Dictionary<BulletModel, ProjectileView> _views = new();

        public List<BulletModel> ActiveProjectiles => _views.Keys.ToList();

        public ProjectileManager(
            SignalBus signalBus,
            ProjectileView.Factory factory,
            PlayerConfig playerConfig,
            ProjectilePool projectilePool)
        {
            _signalBus = signalBus;
            _factory = factory;
            _playerConfig = playerConfig;
            _projectilePool = projectilePool;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        public void Tick()
        {
            float dt = Time.deltaTime;
            var bullets = _views.Keys.ToList();
            var toDestroy = new List<BulletModel>();

            foreach (var bullet in bullets)
            {
                if (!bullet.IsActive) { toDestroy.Add(bullet); continue; }

                bullet.Body.UpdatePhysics(dt);
                bullet.LifeTime -= dt;

                if (bullet.LifeTime <= 0)
                {
                    bullet.IsActive = false;
                    toDestroy.Add(bullet);
                }
            }

            // Уничтожаем ПОСЛЕ итерации — чтобы CollisionManager успел проверить в этом кадре
            foreach (var bullet in toDestroy)
                _projectilePool.Release(bullet);
        }

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views[signal.Bullet] = view;
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                UnityEngine.Object.Destroy(view.gameObject);
                _views.Remove(signal.Bullet);
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Unsubscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }
    }
}