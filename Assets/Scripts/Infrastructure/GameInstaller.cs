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
            SignalBusInstaller.Install(Container);
            var loader = new ConfigLoader();
            bool useMobileInput = Application.isMobilePlatform || loader.World.ForceMobileInput;
            
            Container.Bind<PlayerConfig>().FromInstance(loader.Player).AsSingle();
            Container.Bind<WorldConfig>().FromInstance(loader.World).AsSingle();
            Container.Bind<List<EnemyConfig>>().FromInstance(loader.Enemies).AsSingle();
            Container.Bind<BulletSettings>().FromInstance(loader.Bullet).AsSingle();
            
            Container.Bind<ConfigLoader>().FromInstance(loader).AsSingle();
            
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