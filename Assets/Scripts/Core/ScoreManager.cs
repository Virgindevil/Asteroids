using Game.Core;
using System.Collections.Generic;
using Zenject;

public class ScoreManager : IInitializable, System.IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly Dictionary<string, int> _rewardByType;
    private int _currentScore;

    public ScoreManager(SignalBus signalBus, List<EnemyConfig> enemyConfigs)
    {
        _signalBus = signalBus;
        _rewardByType = new Dictionary<string, int>();

        foreach (var enemy in enemyConfigs)
        {
            _rewardByType[enemy.EnemyType.ToString()] = enemy.ScoreReward;
        }
    }

    public void Initialize() => _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        if (signal.Enemy?.Config == null) return;

        if (_rewardByType.TryGetValue(signal.Enemy.Config.EnemyType.ToString(), out int reward))
        {
            _currentScore += reward;
            _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
        }
    }

    public void Dispose() => _signalBus.TryUnsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
}
