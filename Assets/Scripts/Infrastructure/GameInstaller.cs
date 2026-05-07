using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Регистрируем ConfigLoader в контейнере — Zenject создаёт его сам
            // NonLazy — создаётся сразу при старте, не при первом запросе
            Container.Bind<ConfigLoader>().AsSingle().NonLazy();

            // Конфиги получаем через FromMethod — Zenject резолвит ConfigLoader
            // и вызывает лямбду после того как все биндинги зарегистрированы
            Container.Bind<PlayerConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Player)
                .AsSingle();
            Container.Bind<WorldConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().World)
                .AsSingle();
            Container.Bind<List<EnemyConfig>>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Enemies)
                .AsSingle();
            Container.Bind<BulletSettings>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Bullet)
                .AsSingle();

            // ForceMobileInput читаем до биндингов через прямое создание —
            // это единственное исключение, потому что нужно ДО выбора стратегии ввода
            var tempLoader = new ConfigLoader();
            var useMobileInput = Application.isMobilePlatform || tempLoader.World.ForceMobileInput;


            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsAdapter>().AsSingle();

            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<PlayerViewModel>().AsSingle();
            Container.Bind<ProjectilePool>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<GameStateService>().AsSingle();
            Container.Bind<GameSessionFacade>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPhysicsTicker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletLifecycleService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySimulator>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdMobService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAimController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerWeaponController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSignals>().AsSingle();

            if (useMobileInput)
                Container.BindInterfacesAndSelfTo<MobileInputStrategy>().AsSingle();
            else
                Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();

            Container.DeclareSignal<PlayerHealthChangedSignal>();
            Container.DeclareSignal<LaserStateChangedSignal>();
            Container.DeclareSignal<BulletCreatedSignal>();
            Container.DeclareSignal<BulletDestroyedSignal>();
            Container.DeclareSignal<EnemyCreatedSignal>();
            Container.DeclareSignal<EnemyDestroyedSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<InvincibleEffectActiveSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<PlayerRevivedSignal>();
        }
    }
}