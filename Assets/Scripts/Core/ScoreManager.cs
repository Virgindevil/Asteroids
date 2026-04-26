using Game.Core;
using System.Collections.Generic;
using Zenject;

public class ScoreManager : IInitializable, System.IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly Dictionary<string, int> _rewardByType;
    private int _currentScore;

    public ScoreManager(SignalBus signalBus, WorldConfig worldConfig)
    {
        _signalBus = signalBus;

        // Заполняем словарь из конфига
        _rewardByType = new Dictionary<string, int>();
        foreach (var enemy in worldConfig.Enemies)
            _rewardByType[enemy.EnemyType] = enemy.ScoreReward;
    }

    public void Initialize() => _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        if (signal.Enemy?.Config == null) return;
        
        if (_rewardByType.TryGetValue(signal.Enemy.Config.EnemyType, out int reward))
        {
            _currentScore += reward;
            _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
        }
    }

    public void Dispose() => _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
}