using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redpenguin.AssetsManagement
{
    internal class AssetsDb
    {
        private readonly Dictionary<Type, Dictionary<string, Object>> _assets = new();
        
        public bool TryAddAsset(Type assetType, Object asset)
        {
            if (!_assets.TryGetValue(assetType, out var assets))
            {
                assets = new Dictionary<string, Object>();
                _assets.Add(assetType, assets);
            }

            if (assets.TryAdd(asset.name, asset))
            {
                Debug.Log($"[AssetsDb] Asset was added {asset.name}");
                return true;
            }

            Debug.Log($"[AssetsDb] Cant add asset {asset.name}");
            return false;
        }
        
        public Object GetAsset(Type assetType, string name)
        {
            if (!_assets.TryGetValue(assetType, out var assets)) 
                return null;
            return assets.GetValueOrDefault(name);
        }
        
        public bool HasAsset(Type assetType, string name)
        {
            if (!_assets.TryGetValue(assetType, out var assets)) 
                return false;
            if (!assets.TryGetValue(name, out var asset))
                return false;
            return true;
        }

        public void RemoveAsset(Type assetType, string name)
        {
            if (!_assets.TryGetValue(assetType, out var assets)) 
                return;
            assets.Remove(name);
        }
    }
}