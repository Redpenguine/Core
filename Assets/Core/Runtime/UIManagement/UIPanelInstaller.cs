using Zenject;

namespace Redpenguin.UIManagement
{
    public class UIPanelInstaller : Installer<UIPanelInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUIPanelProvider>().To<UIPanelProvider>().AsSingle();
            Container.Bind<IUIPanelFacade>().To<UIPanelFacade>().AsSingle();
        }
    }
}