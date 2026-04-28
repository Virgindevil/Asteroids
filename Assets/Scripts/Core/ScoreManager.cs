using Game.Core;
using System.Collections.Generic;
using Zenject;

public class ScoreManager : IInitializable, System.IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly Dictionary<string, int> _rewardByType;
    private int _currentScore;

    // Теперь внедряем List<EnemyConfig> напрямую
    public ScoreManager(SignalBus signalBus, List<EnemyConfig> enemyConfigs)
    {
        _signalBus = signalBus;
        _rewardByType = new Dictionary<string, int>();

        if (enemyConfigs == null)
        {
            UnityEngine.Debug.LogError("[ScoreManager] EnemyConfigs list is NULL!");
            return;
        }

        // Заполняем словарь из списка конфигов врагов
        foreach (var enemy in enemyConfigs)
        {
            // Используем TryAdd или проверку, так как в JSON у вас два "Asteroid"
            // Если ключи повторяются, обычный [] перезапишет значение
            _rewardByType[enemy.EnemyType] = enemy.ScoreReward;
        }
    }

    public void Initialize() => _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        // Проверяем, что сигнал и конфиг врага существуют
        if (signal.Enemy?.Config == null) return;

        if (_rewardByType.TryGetValue(signal.Enemy.Config.EnemyType, out int reward))
        {
            _currentScore += reward;
            _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
        }
    }

    public void Dispose() => _signalBus.TryUnsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
}
