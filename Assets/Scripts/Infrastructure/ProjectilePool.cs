using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class ProjectilePool
    {
        private readonly BulletSettings _bulletSettings;
        private readonly float _bulletSpeed;
        private readonly ObjectPool<BulletModel> _pool;
        private readonly SignalBus _signalBus;
        private readonly float _bulletLifeTime;

        public ProjectilePool(SignalBus signalBus, PlayerConfig config, BulletSettings bulletSettings)
        {
            _bulletSettings = bulletSettings;
            _signalBus = signalBus;
            _bulletLifeTime = config.BulletLifeTime;
            _bulletSpeed = config.BulletSpeed;

            _pool = new ObjectPool<BulletModel>(
                () => new BulletModel(Vector2.zero, Vector2.zero, _bulletSettings),
                b => b.IsActive = true,
                b => b.IsActive = false,
                10
            );
        }

        public void Spawn(Vector2 position, Vector2 direction)
        {
            var bullet = _pool.Get();
            bullet.Body.ResetState(position, direction * _bulletSpeed);
            bullet.LifeTime = _bulletLifeTime;
            _signalBus.Fire(new BulletCreatedSignal { Bullet = bullet });
        }

        public void Release(BulletModel bullet)
        {
            _pool.Release(bullet);
            _signalBus.Fire(new BulletDestroyedSignal { Bullet = bullet });
        }
    }
}