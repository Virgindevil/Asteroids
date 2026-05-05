namespace Game.Core
{
    public struct PlayerHealthChangedSignal { public int CurrentHealth; }
    
    public struct EnemyCreatedSignal {public EnemyModel Enemy; }
    public struct EnemyDestroyedSignal {public EnemyModel Enemy; }
    
    public struct LaserStateChangedSignal { public bool IsActive; }
    
    public struct BulletCreatedSignal { public BulletModel Bullet; }
    public struct BulletDestroyedSignal { public BulletModel Bullet; }
    
    public struct ScoreChangedSignal { public int TotalScore; }
    
    public struct InvincibleEffectActiveSignal { public bool IsActive; }
    
    public struct GameOverSignal { }
    public struct PlayerRevivedSignal { }
}