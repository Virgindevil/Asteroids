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
            Container.Bind<ConfigLoader>().AsSingle().NonLazy();
            SignalBusInstaller.Install(Container);
            bool useMobileInput = Application.isMobilePlatform || Container.Resolve<ConfigLoader>().World.ForceMobileInput;
            
            Container.Bind<WorldConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().World)
                .AsSingle();
            Container.Bind<PlayerConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Player)
                .AsSingle();
            Container.Bind<List<EnemyConfig>>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Enemies)
                .AsSingle();
            Container.Bind<BulletSettings>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Bullet)
                .AsSingle();
            
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
            Container.BindInterfacesAndSelfTo<BulletPhysicsTicker>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyRegistry>().AsSingle();              // CollisionManager получит через интерфейс
            Container.BindInterfacesAndSelfTo<EnemySimulator>().AsSingle(); // апдейт и смерть
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