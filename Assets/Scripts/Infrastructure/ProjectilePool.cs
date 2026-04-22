using UnityEngine;
using Game.Core;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

namespace Game.Infrastructure
{
    public class ProjectilePool
    {
        private readonly SignalBus _signalBus;

        public ProjectilePool(SignalBus signalBus) => _signalBus = signalBus;

        public void Spawn(Vector2 position, Vector2 direction)
        {
            // Создаем модель
            var bullet = new BulletModel(position, direction);
        
            // Отправляем сигнал. ProjectileManager его поймает и создаст View
            _signalBus.Fire(new BulletCreatedSignal { Bullet = bullet });
        }
    }
}