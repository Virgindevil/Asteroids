namespace Game.Core
{
    // Сигнал об изменении здоровья
    public struct PlayerHealthChangedSignal { public int CurrentHealth; }
    
    // Сигнал столкновения (для искр/партиклов)
    public struct CollisionOccurredSignal { public UnityEngine.Vector2 Position; }

    // Сигнал выстрела лазером
    public struct LaserFiredSignal { public int RemainingCharges; }
}