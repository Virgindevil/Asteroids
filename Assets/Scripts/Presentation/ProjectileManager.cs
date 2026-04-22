using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class ProjectileManager : IInitializable, IDisposable, IProjectileProvider, ITickable
    {
        private readonly SignalBus _signalBus;
        private readonly ProjectileView.Factory _factory;
        private readonly Dictionary<BulletModel, ProjectileView> _views = new();
        // Реализация интерфейса
        public List<BulletModel> ActiveProjectiles => _views.Keys.ToList();

        public ProjectileManager(SignalBus signalBus, ProjectileView.Factory factory)
        {
            _signalBus = signalBus;
            _factory = factory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }
        
        public void Tick()
        {
            float dt = Time.deltaTime;
            // Копируем список ключей, чтобы спокойно удалять элементы из словаря во время итерации
            var bullets = _views.Keys.ToList();

            foreach (var bullet in bullets)
            {
                // 1. Обновляем физику
                bullet.Body.UpdatePhysics(dt);

                // 2. Уменьшаем время жизни
                bullet.LifeTime -= dt;

                // 3. Если пуля "протухла" или флаг IsActive стал false (после коллизии)
                if (bullet.LifeTime <= 0 || !bullet.IsActive)
                {
                    // Сообщаем всем (в том числе самому себе через подписку), что пуля уничтожена
                    _signalBus.Fire(new BulletDestroyedSignal { Bullet = bullet });
                }
            }
        }

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views.Add(signal.Bullet, view);
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                UnityEngine.Object.Destroy(view.gameObject); // Или возврат в пул вьюх
                _views.Remove(signal.Bullet);
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Unsubscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }
    }
}