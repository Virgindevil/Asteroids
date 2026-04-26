using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class ProjectilePool
    {
        private readonly SignalBus _signalBus;
        private readonly ObjectPool<BulletModel> _pool;

        public ProjectilePool(SignalBus signalBus, PlayerConfig config)
        {
            _signalBus = signalBus;
            _pool = new ObjectPool<BulletModel>(
                factory: () => new BulletModel(Vector2.zero, Vector2.zero),
                onGet: b => b.IsActive = true,
                onRelease: b => b.IsActive = false,
                initialSize: 10
            );
        }

        public void Spawn(Vector2 position, Vector2 direction)
        {
            var bullet = _pool.Get();
            bullet.Body.ResetState(position, direction * 15f);
            bullet.LifeTime = 2f;
            _signalBus.Fire(new BulletCreatedSignal { Bullet = bullet });
        }

        public void Release(BulletModel bullet)
        {
            _pool.Release(bullet);
            _signalBus.Fire(new BulletDestroyedSignal { Bullet = bullet });
        }
    }
}