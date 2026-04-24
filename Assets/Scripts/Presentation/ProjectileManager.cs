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
        // Внедряем конфиг игрока
        private readonly PlayerConfig _playerConfig;

        private readonly Dictionary<BulletModel, ProjectileView> _views = new();

        public List<BulletModel> ActiveProjectiles => _views.Keys.ToList();

        // Добавляем PlayerConfig в конструктор
        public ProjectileManager(
            SignalBus signalBus,
            ProjectileView.Factory factory,
            PlayerConfig playerConfig)
        {
            _signalBus = signalBus;
            _factory = factory;
            _playerConfig = playerConfig;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BulletCreatedSignal>(OnBulletCreated);
            _signalBus.Subscribe<BulletDestroyedSignal>(OnBulletDestroyed);
        }

        public void Tick()
        {
            float dt = Time.deltaTime;
            var bullets = _views.Keys.ToList();

            foreach (var bullet in bullets)
            {
                // Пример использования: если скорость пули не задана в самой модели BulletModel,
                // мы могли бы применять её здесь, но обычно скорость задается при создании пули.
                // Однако, здесь мы можем использовать _playerConfig для логики валидации или модификаторов.

                bullet.Body.UpdatePhysics(dt);
                bullet.LifeTime -= dt;

                if (bullet.LifeTime <= 0 || !bullet.IsActive)
                {
                    _signalBus.Fire(new BulletDestroyedSignal { Bullet = bullet });
                }
            }
        }

        private void OnBulletCreated(BulletCreatedSignal signal)
        {
            // Если BulletModel создается без начальной скорости, 
            // мы можем принудительно установить её из конфига здесь:
            //signal.Bullet.Body.Velocity = signal.Bullet.Body.Direction * _playerConfig.BulletSpeed;

            var view = _factory.Create();
            view.Initialize(signal.Bullet);
            _views.Add(signal.Bullet, view);
        }

        private void OnBulletDestroyed(BulletDestroyedSignal signal)
        {
            if (_views.TryGetValue(signal.Bullet, out var view))
            {
                UnityEngine.Object.Destroy(view.gameObject);
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
