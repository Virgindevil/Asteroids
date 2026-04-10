using System;
using System.Collections.Generic;
using Game.Core;
using Zenject;

namespace Game.Presentation
{
    public class ProjectileManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ProjectileView.Factory _factory;
        private readonly Dictionary<BulletModel, ProjectileView> _views = new();

        public ProjectileManager(SignalBus signalBus, ProjectileView.Factory factory)
        {
            _signalBus = signalBus;
            _factory = factory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views.Add(signal.Bullet, view);
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                UnityEngine.Object.Destroy(view.gameObject); // Или возврат в пул вьюх
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