using Game.Core;
using Game.Presentation;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyViewManager : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly AsteroidView.Factory _asteroidFactory;
    private readonly UfoView.Factory _ufoFactory;

    // Словарь для связи модели и её визуального объекта
    private readonly Dictionary<EnemyModel, MonoBehaviour> _views = new();

    public EnemyViewManager(SignalBus signalBus, AsteroidView.Factory asteroidFactory, UfoView.Factory ufoFactory)
    {
        _signalBus = signalBus;
        _asteroidFactory = asteroidFactory;
        _ufoFactory = ufoFactory;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<EnemyCreatedSignal>(OnEnemyCreated);
        _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed); // Добавлена подписка
    }

    private void OnEnemyCreated(EnemyCreatedSignal signal)
    {
        MonoBehaviour view;

        if (signal.Enemy is UfoModel)
        {
            var ufoView = _ufoFactory.Create(signal.Enemy.Body.Position);
            ufoView.Initialize(signal.Enemy);
            view = ufoView;
        }
        else
        {
            var asteroidView = _asteroidFactory.Create(signal.Enemy.Body.Position);
            asteroidView.Initialize(signal.Enemy);
            view = asteroidView;
        }

        // Сохраняем вьюху в словарь, чтобы потом знать, что удалять
        if (!_views.ContainsKey(signal.Enemy))
        {
            _views.Add(signal.Enemy, view);
        }
    }

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        // Ищем вьюху по модели из сигнала
        if (_views.TryGetValue(signal.Enemy, out var view))
        {
            if (view != null && view.gameObject != null)
            {
                UnityEngine.Object.Destroy(view.gameObject);
            }
            _views.Remove(signal.Enemy);
        }
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyCreatedSignal>(OnEnemyCreated);
        _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }
}