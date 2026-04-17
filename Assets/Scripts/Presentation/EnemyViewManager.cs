using Game.Core;
using Game.Presentation;
using System;
using Zenject;

public class EnemyViewManager : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly AsteroidView.Factory _asteroidFactory;
    private readonly UfoView.Factory _ufoFactory;

    public EnemyViewManager(SignalBus signalBus, AsteroidView.Factory asteroidFactory, UfoView.Factory ufoFactory)
    {
        _signalBus = signalBus;
        _asteroidFactory = asteroidFactory;
        _ufoFactory = ufoFactory;
    }

    public void Initialize() => _signalBus.Subscribe<EnemyCreatedSignal>(OnEnemyCreated);

    private void OnEnemyCreated(EnemyCreatedSignal signal)
    {
        // Вот тут магия разделения:
        if (signal.Enemy is UfoModel)
        {
            var view = _ufoFactory.Create(signal.Enemy.Body.Position);
            view.Initialize(signal.Enemy);
        }
        else
        {
            var view = _asteroidFactory.Create(signal.Enemy.Body.Position);
            view.Initialize(signal.Enemy);
        }
    }

    public void Dispose() => _signalBus.Unsubscribe<EnemyCreatedSignal>(OnEnemyCreated);
}