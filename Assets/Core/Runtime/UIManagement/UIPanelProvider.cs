using System;
using System.Collections.Generic;
using UnityEngine;

namespace Redpenguin.UIManagement
{
    public interface IUIPanelProvider
    {
        void RegisterPanel<T>(T panel) where T : IUIPanel;
        T GetPanel<T>() where T : IUIPanel;
    }

    public class UIPanelProvider : IUIPanelProvider
    {
        private readonly Dictionary<Type, IUIPanel> _panels = new();
        
        public void RegisterPanel<T>(T panel) where T : IUIPanel
        {
            var type = panel.GetType();
            if (_panels.TryAdd(type, panel) == false)
            {
                Debug.LogError($"Panel {type} already registered");
            }
        }
        
        public T GetPanel<T>() where T : IUIPanel
        {
            if (_panels.TryGetValue(typeof(T), out var panel))
            {
                return (T) panel;
            }
            Debug.LogError($"Panel {typeof(T)} not found");
            return default;
        }
    }
}