using Game.Core;
using Zenject;

public class ScoreManager : IInitializable, System.IDisposable
{
    private readonly SignalBus _signalBus;
    private int _currentScore;

    public ScoreManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        // Берем награду прямо из конфига уничтоженной модели
        if (signal.Enemy != null && signal.Enemy.Config != null)
        {
            _currentScore += signal.Enemy.Config.ScoreReward;
            
            // Рассылаем сигнал с обновленным счетом
            _signalBus.Fire(new ScoreChangedSignal { TotalScore = _currentScore });
        }
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }
}