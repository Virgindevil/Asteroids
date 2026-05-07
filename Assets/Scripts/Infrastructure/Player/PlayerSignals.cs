using System;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class PlayerSignals : IInitializable, IDisposable
    {
        private readonly PlayerModel _model;
        private readonly SignalBus _signalBus;

        public PlayerSignals(PlayerModel model, SignalBus signalBus)
        {
            _model = model;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<PlayerRevivedSignal>(_model.Revive);
            _signalBus.TryUnsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerRevivedSignal>(_model.Revive);
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        private void OnHealthChanged(PlayerHealthChangedSignal signal)
        {
            if (signal.CurrentHealth <= 0)
                _signalBus.Fire(new GameOverSignal());
        }
    }
}