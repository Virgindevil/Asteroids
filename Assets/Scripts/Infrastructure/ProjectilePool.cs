using UnityEngine;
using Game.Core;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

namespace Game.Infrastructure
{
    // Нам больше не нужен ITickable!
    public class ProjectilePool
    {
        private readonly SignalBus _signalBus;

        public ProjectilePool(SignalBus signalBus) => _signalBus = signalBus;

        public void Spawn(Vector2 position, Vector2 direction)
        {
            FireBulletTask(position, direction).Forget(); // Запускаем и забываем (fire and forget)
        }

        private async UniTaskVoid FireBulletTask(Vector2 position, Vector2 direction)
        {
            var bullet = new BulletModel(position, direction);
            
            _signalBus.Fire(new BulletCreatedSignal { Bullet = bullet });

            // Пуля летит сама 2 секунды
            await bullet.RunLifeCycle(TimeSpan.FromSeconds(2));

            _signalBus.Fire(new BulletDestroyedSignal { Bullet = bullet });
        }
    }
}