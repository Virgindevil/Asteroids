using System;
using System.Collections.Generic;

namespace Game.Core
{
    public enum EnemyType { Asteroid, UFO }
    
    [Serializable]
    public class PlayerConfig
    {
        public float CollisionRadius;
        public float MaxHealth;
        public float MovementAcceleration;
        public float Friction;
        public int MaxLaserCharges;
        public float LaserCooldown;
        public float BulletSpeed;
        public float BulletLifeTime;
        public float LaserLength;
        public float LaserDamage;
        public float LaserActiveDuration;
        public float LaserCooldownDelay;
        public float ShootCooldown;
        public float InvulnerabilityDuration;
    }
    
    [Serializable]
    public class BulletSettings
    {
        public int BulletDamage;
        public float CollisionRadius;
    }

    [Serializable]
    public class EnemyConfig
    {
        public EnemyType EnemyType;
        public float Speed;
        public float Health;
        public int ScoreReward;
        public bool CanSplit;
        public float Fragments;
        public float CollisionRadius;
        public float Friction;
        public float FragmentSpeedBoost;
        public float FragmentRadiusMultiplier;
    }

    [Serializable]
    public class WorldConfig
    {
        public float Width;
        public float Height;
        public int MaxEnemies;
        public float EnemiesSpawnInterval;
        public bool ForceMobileInput;
        public float FrictionTimeMultiplier;
        public float TeleportBoundaryOffset;
        public float SpawnAreaDivisor;
    }
}
