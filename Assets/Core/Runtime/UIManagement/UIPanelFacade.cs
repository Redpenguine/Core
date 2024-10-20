namespace Redpenguin.UIManagement
{
    public interface IUIPanelFacade
    {
        T GetPanel<T>() where T : IUIPanel;
    }

    public class UIPanelFacade : IUIPanelFacade
    {
        private readonly IUIPanelProvider _panelProvider;

        public UIPanelFacade(IUIPanelProvider panelProvider)
        {
            _panelProvider = panelProvider;
        }
        
        public T GetPanel<T>() where T : IUIPanel
        {
            return _panelProvider.GetPanel<T>();
        }
    }
}