// Presentation/ProjectileManager.cs
using System;
using System.Collections.Generic;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    // ITickable убран — ProjectileManager больше не тикает
    public class ProjectileManager : IInitializable, IDisposable, IProjectileProvider
    {
        private readonly SignalBus _signalBus;
        private readonly ProjectileView.Factory _factory;
        private readonly ProjectilePool _projectilePool;

        private readonly Dictionary<BulletModel, ProjectileView> _views = new();
        private readonly List<BulletModel> _activeBullets = new();

        public IReadOnlyList<BulletModel> ActiveProjectiles => _activeBullets;

        // _playerConfig убран — он не нужен
        public ProjectileManager(
            SignalBus signalBus,
            ProjectileView.Factory factory,
            ProjectilePool projectilePool)
        {
            _signalBus = signalBus;
            _factory = factory;
            _projectilePool = projectilePool;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        // Tick полностью удалён

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            // Регистрируем пулю и создаём View
            _activeBullets.Add(signal.Bullet);

            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views[signal.Bullet] = view;
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            // BulletLifecycleService вызвал pool.Release → сигнал пришёл сюда
            // Удаляем пулю из активных и уничтожаем View
            _activeBullets.Remove(signal.Bullet);

            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                if (view != null)
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