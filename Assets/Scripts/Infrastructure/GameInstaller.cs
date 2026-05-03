using Game.Core;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // 1. Создаем лоадер. Конструктор сразу загрузит файлы.
            var loader = new ConfigLoader();
            
            SignalBusInstaller.Install(Container);

            bool useMobileInput = Application.isMobilePlatform || loader.World.ForceMobileInput;

            // 2. Биндим сами данные. 
            // Используйте IfNotBound, чтобы избежать конфликтов, если они есть
            Container.Bind<PlayerConfig>().FromInstance(loader.Player).AsSingle();
            Container.Bind<WorldConfig>().FromInstance(loader.World).AsSingle();
            Container.Bind<List<EnemyConfig>>().FromInstance(loader.Enemies).AsSingle();

            // 4. Все остальное
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<PlayerViewModel>().AsSingle();
            Container.Bind<ProjectilePool>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<GameStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPhysicsTicker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletPhysicsTicker>().AsSingle();

            if (useMobileInput)
                Container.BindInterfacesAndSelfTo<MobileInputStrategy>().AsSingle();
            else
                Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();

            // Регистрация типов сигналов
            Container.DeclareSignal<PlayerHealthChangedSignal>();
            Container.DeclareSignal<LaserFiredSignal>();
            Container.DeclareSignal<LaserStateChangedSignal>();            
            Container.DeclareSignal<BulletCreatedSignal>();
            Container.DeclareSignal<BulletDestroyedSignal>();
            Container.DeclareSignal<EnemyCreatedSignal>();
            Container.DeclareSignal<EnemyDestroyedSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<InvincibleEffectActiveSignal>();
            // Сигналы
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<PlayerRevivedSignal>();

            // Сервисы
            Container.BindInterfacesAndSelfTo<AdMobService>().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsAdapter>().AsSingle();


            // Регистрация менеджера очков
            Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
            
            // Для врагов
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            
            Container.Bind<EnemyFacade>().AsSingle();

            // Для коллизий
            Container.BindInterfacesAndSelfTo<CollisionManager>().AsSingle().NonLazy();

        }
    }
}