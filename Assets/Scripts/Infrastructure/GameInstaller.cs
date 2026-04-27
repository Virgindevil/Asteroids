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
            Container.Bind<EnemyFactory>().AsSingle();

            // Заменяем старую строку регистрации ввода на эту логику:
            #if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR 
            // В редакторе тоже удобно оставить Mobile, чтобы тестировать мышкой кнопки
            if (Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld)
            {
                Container.BindInterfacesAndSelfTo<MobileInputStrategy>().AsSingle();
            }
            else
            {
                Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();
            }
            #else
                Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();
            #endif

            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();

            // Регистрация типов сигналов
            Container.DeclareSignal<PlayerHealthChangedSignal>();
            Container.DeclareSignal<CollisionOccurredSignal>();
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
            Container.Bind<IAdsService>().To<AdMobService>().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsAdapter>().AsSingle();


            // Регистрация менеджера очков
            Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
            
            // Для врагов
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            
            Container.Bind<EnemyFacade>().AsSingle();

        }
    }
}