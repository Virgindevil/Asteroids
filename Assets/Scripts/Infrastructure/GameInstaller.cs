using Zenject;
using Game.Core;

namespace Game.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Привязываем и как ConfigLoader, и как IInitializable
            Container.BindInterfacesAndSelfTo<ConfigLoader>().AsSingle().NonLazy();

            // Биндинг подобъектов теперь должен быть "ленивым", 
            // так как в момент InstallBindings они еще null
            Container.Bind<PlayerConfig>().FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Root.Player).AsCached();
            Container.Bind<WorldConfig>().FromMethod(ctx => ctx.Container.Resolve<ConfigLoader>().Root.World).AsCached();
        }
    }
}