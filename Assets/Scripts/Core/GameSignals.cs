namespace Game.Core
{
    // Сигнал об изменении здоровья
    public struct PlayerHealthChangedSignal { public int CurrentHealth; }
    
    public struct EnemyCreatedSignal {public EnemyModel Enemy; }
    public struct EnemyDestroyedSignal {public EnemyModel Enemy; }
    // Сигнал столкновения (для искр/партиклов)

    // Сигнал выстрела лазером
    public struct LaserFiredSignal { public int RemainingCharges; }
    public struct LaserStateChangedSignal { public bool IsActive; }
    
    // Передаем саму модель, чтобы View знала, за кем следить
    public struct BulletCreatedSignal { public BulletModel Bullet; }
    public struct BulletDestroyedSignal { public BulletModel Bullet; }
    

    public struct CollisionOccurredSignal { public UnityEngine.Vector2 Position; }
}