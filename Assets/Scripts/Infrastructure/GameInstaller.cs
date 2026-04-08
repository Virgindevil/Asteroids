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

            if (configLoader.Root != null)
            {
                Container.Bind<PlayerConfig>().FromInstance(configLoader.Root.Player).AsSingle();
                Container.Bind<WorldConfig>().FromInstance(configLoader.Root.World).AsSingle();
            }
            else
            {
                Debug.LogError("КРИТИЧЕСКАЯ ОШИБКА: Конфиги не загружены! Проверь JSON.");
            }

            // 3. Остальные зависимости
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<PlayerViewModel>().AsSingle();
            Container.Bind<IInputStrategy>().To<KeyboardInputStrategy>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
    }
}