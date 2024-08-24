using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Redpenguin.AssetsManagement
{
    public interface IAssetsProvider
    {
        public UniTask LoadAssets(IAssetsHolder assetsHolder, params ILoadRequest[] loadRequests);
        public T GetAsset<T>(string name) where T : Object;
        public bool HasAsset<T>(string name) where T : Object;
        public void ReleaseAssets(IAssetsHolder assetsHolder);
    }

    public static class AssetsProviderExtensions
    {
        public static T GetScriptableObject<T>(this IAssetsProvider assetsProvider, string name) where T : ScriptableObject
        {
            return assetsProvider.GetAsset<ScriptableObject>(name) as T;
        }
        
        public static T GetPrefab<T>(this IAssetsProvider assetsProvider, string name) where T : MonoBehaviour
        {
            return assetsProvider.GetAsset<GameObject>(name).GetComponent<T>();
        }
    }
}