using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class GameSettingsRoot
    {
        public PlayerConfig Player;
        public WorldConfig World;
    }

    [Serializable]
    public class PlayerConfig
    {
        public float MaxHealth;
        public float MovementAcceleration;
        public float Friction; // Трение для инерции
        public int MaxLaserCharges;
        public float LaserCooldown;
        public float BulletSpeed;
        public float ShootCooldown;
    }

    [Serializable]
    public class EnemyConfig
    {
        public string EnemyType; // "AsteroidBig", "AsteroidSmall", "UFO"
        public float Speed;
        public float Health;
        public int ScoreReward;
        public bool CanSplit;
        public float CollisionRadius;
        public float Friction;
    }

    [Serializable]
    public class WorldConfig
    {
        public float Width;
        public float Height;
        public int MaxEnemies;
        public List<EnemyConfig> Enemies;
    }
}
