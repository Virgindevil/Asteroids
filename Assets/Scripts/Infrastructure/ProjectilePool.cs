using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class ProjectilePool
    {
        private readonly List<BulletModel> _activeBullets = new();
        private readonly Stack<BulletModel> _pool = new();
        private readonly PlayerConfig _config;

        public ProjectilePool(PlayerConfig config)
        {
            _config = config;
        }

        public BulletModel Spawn(Vector2 position, Vector2 direction)
        {
            BulletModel bullet;
            Vector2 velocity = direction * _config.BulletSpeed;

            if (_pool.Count > 0)
            {
                bullet = _pool.Pop();
                bullet.IsActive = true;
                bullet.Body.ResetState(position, velocity);
            }
            else
            {
                bullet = new BulletModel(position, velocity);
            }

            _activeBullets.Add(bullet);
            return bullet;
        }

        public void Despawn(BulletModel bullet)
        {
            bullet.IsActive = false;
            _activeBullets.Remove(bullet);
            _pool.Push(bullet);
        }

        public void Update(float dt)
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                var bullet = _activeBullets[i];
                bullet.Update(dt);
                if (!bullet.IsActive) Despawn(bullet);
            }
        }

        public IReadOnlyList<BulletModel> ActiveBullets => _activeBullets;
    }
}