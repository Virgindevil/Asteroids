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
        private readonly Dictionary<EnemyModel, AsteroidView> _views = new();

        public EnemyViewManager(SignalBus signalBus, AsteroidView.Factory factory)
        {
            _signalBus = signalBus;
            _factory = factory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyCreatedSignal>(OnEnemyCreated);
        }

        private void OnEnemyCreated(EnemyCreatedSignal signal)
        {
            var view = _factory.Create();
            // signal.Enemy теперь типа EnemyModel, Initialize его примет
            view.Initialize(signal.Enemy);
            _views.Add(signal.Enemy, view);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyCreatedSignal>(OnEnemyCreated);
        }
    }
}