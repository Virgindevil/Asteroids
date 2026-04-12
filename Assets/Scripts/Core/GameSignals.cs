namespace Game.Core
{
    // Сигнал об изменении здоровья
    public struct PlayerHealthChangedSignal { public int CurrentHealth; }
    
    // Сигнал столкновения (для искр/партиклов)
    public struct CollisionOccurredSignal { public UnityEngine.Vector2 Position; }

    // Сигнал выстрела лазером
    public struct LaserFiredSignal { public int RemainingCharges; }
    
    // Передаем саму модель, чтобы View знала, за кем следить
    public struct BulletCreatedSignal { public BulletModel Bullet; }
    public struct BulletDestroyedSignal { public BulletModel Bullet; }

    public struct LaserStateChangedSignal { public bool IsActive; }
}