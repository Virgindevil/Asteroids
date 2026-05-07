using System;
using System.Collections.Generic;
using Zenject;

namespace Game.Core
{
    public class ScoreManager : IInitializable, IDisposable
    {
        private readonly Dictionary<EnemyType, int> _rewardByType;
        private readonly SignalBus _signalBus;
        private int _currentScore;

        public ScoreManager(SignalBus signalBus, List<EnemyConfig> enemyConfigs)
        {
            _signalBus = signalBus;
            _rewardByType = new Dictionary<EnemyType, int>();

            foreach (var enemy in enemyConfigs)
                _rewardByType[enemy.EnemyType] = enemy.ScoreReward;
        }

        public void Initialize() =>
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            if (signal.Enemy?.Config == null) return;

            if (_rewardByType.TryGetValue(signal.Enemy.Config.EnemyType, out var reward))
            {
                _currentScore += reward;
                _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
            }
        }

        public void Dispose() =>
            _signalBus.TryUnsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }
}