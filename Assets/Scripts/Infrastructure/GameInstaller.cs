using Zenject;
using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // 1. Создаем загрузчик и СРАЗУ вызываем загрузку
            var configLoader = new ConfigLoader();
            configLoader.LoadManually(); // Создадим этот метод сейчас

            // 2. Биндим сам загрузчик и уже готовые данные
            Container.Bind<ConfigLoader>().FromInstance(configLoader).AsSingle();

            Container.Bind<PlayerConfig>().FromInstance(configLoader.Root.Player).AsSingle();
            Container.Bind<WorldConfig>().FromInstance(configLoader.Root.World).AsSingle();

            // 3. Остальные зависимости
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<PlayerViewModel>().AsSingle();
            Container.Bind<ProjectilePool>().AsSingle();
            Container.Bind<AsteroidFactory>().AsSingle();

            Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            
            // Регистрация типов сигналов
            Container.DeclareSignal<PlayerHealthChangedSignal>();
            Container.DeclareSignal<CollisionOccurredSignal>();
            Container.DeclareSignal<LaserFiredSignal>();
            Container.DeclareSignal<LaserStateChangedSignal>();            
            Container.DeclareSignal<BulletCreatedSignal>();
            Container.DeclareSignal<BulletDestroyedSignal>();
            Container.DeclareSignal<AsteroidCreatedSignal>();

        }
    }
}