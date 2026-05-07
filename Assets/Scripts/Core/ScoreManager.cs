using System;
using System.Collections.Generic;
using Game.Core;
using Zenject;

namespace Game.Core
{
    public class ScoreManager : IInitializable, IDisposable
    {
        private readonly Dictionary<string, int> _rewardByType;
        private readonly SignalBus _signalBus;
        private int _currentScore;

        public ScoreManager(SignalBus signalBus, List<EnemyConfig> enemyConfigs)
        {
            _signalBus = signalBus;
            _rewardByType = new Dictionary<string, int>();

            foreach (var enemy in enemyConfigs) _rewardByType[enemy.EnemyType.ToString()] = enemy.ScoreReward;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            if (signal.Enemy?.Config == null) return;

            if (_rewardByType.TryGetValue(signal.Enemy.Config.EnemyType.ToString(), out var reward))
            {
                _currentScore += reward;
                _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
            }
        }
    }
}