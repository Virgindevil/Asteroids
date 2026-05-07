using System;
using System.Collections.Generic;
using Game.Core;
using Game.Infrastructure;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Presentation
{
    public class ProjectileManager : IInitializable, IDisposable, IProjectileProvider
    {
        private readonly List<BulletModel> _activeBullets = new();
        private readonly ProjectileView.Factory _factory;
        private readonly ProjectilePool _projectilePool;
        private readonly SignalBus _signalBus;

        private readonly Dictionary<BulletModel, ProjectileView> _views = new();

        public ProjectileManager(
            SignalBus signalBus,
            ProjectileView.Factory factory,
            ProjectilePool projectilePool)
        {
            _signalBus = signalBus;
            _factory = factory;
            _projectilePool = projectilePool;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Unsubscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        public IReadOnlyList<BulletModel> ActiveProjectiles => _activeBullets;

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            _activeBullets.Add(signal.Bullet);

            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views[signal.Bullet] = view;
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            _activeBullets.Remove(signal.Bullet);

            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                if (view != null)
                    Object.Destroy(view.gameObject);

                _views.Remove(signal.Bullet);
            }
        }
    }
}