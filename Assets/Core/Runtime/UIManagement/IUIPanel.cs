using UnityEngine;

namespace Redpenguin.UIManagement
{
    public interface IUIPanel
    {
    }

    public abstract class UIPanel : MonoBehaviour, IUIPanel
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}