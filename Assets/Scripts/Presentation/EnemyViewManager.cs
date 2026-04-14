using System;
using System.Collections.Generic;
using Game.Core;
using Zenject;

namespace Game.Presentation
{
    public class EnemyViewManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly AsteroidView.Factory _factory;
        private readonly Dictionary<AsteroidModel, AsteroidView> _views = new();

        public EnemyViewManager(SignalBus signalBus, AsteroidView.Factory factory)
        {
            _signalBus = signalBus;
            _factory = factory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<AsteroidCreatedSignal>(OnAsteroidCreated);
        }

        private void OnAsteroidCreated(AsteroidCreatedSignal signal)
        {
            var view = _factory.Create();
            view.Initialize(signal.Asteroid);
            _views.Add(signal.Asteroid, view);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<AsteroidCreatedSignal>(OnAsteroidCreated);
        }
    }
}