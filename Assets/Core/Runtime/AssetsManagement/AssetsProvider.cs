using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Redpenguin.AssetsManagement
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly AssetsDb _assetsDb;
        private readonly LoadRequestsDb _loadRequestsDb;
        private readonly AssetsHolderDb _assetsHolderDb;
        private readonly AssetsLoader _assetsLoader;

        public AssetsProvider()
        {
            _assetsDb = new AssetsDb();
            _loadRequestsDb = new LoadRequestsDb();
            _assetsHolderDb = new AssetsHolderDb();
            _assetsLoader = new AssetsLoader(_loadRequestsDb, _assetsHolderDb, _assetsDb);
        }

        public async UniTask LoadAssets(IAssetsHolder assetsHolder, params ILoadRequest[] loadRequests)
        {
            var loadTasks = new List<UniTask>();
            foreach (var loadRequest in loadRequests)
            {
                loadTasks.Add(_assetsLoader.LoadAssets(assetsHolder, loadRequest));
            }

            await UniTask.WhenAll(loadTasks);
        }

        public T GetAsset<T>(string name) where T : Object
        {
            return _assetsDb.GetAsset(typeof(T), name) as T;
        }

        public bool HasAsset<T>(string name) where T : Object
        {
            return _assetsDb.HasAsset(typeof(T), name);
        }

        public void ReleaseAssets(IAssetsHolder assetsHolder)
        {
            if (!_assetsHolderDb.TryGetLoadRequests(assetsHolder, out var loadRequests))
                return;
            RemoveLoadRequestFromDb(loadRequests);
            _assetsHolderDb.RemoveAssetsHolder(assetsHolder);
        }

        private void RemoveLoadRequestFromDb(List<ILoadRequest> loadRequests)
        {
            foreach (var loadRequest in loadRequests)
            {
                if (!_loadRequestsDb.TryGetLoadRequests(loadRequest, out var handleWrappers))
                    continue;
                RemoveAssetsFromDb(handleWrappers, loadRequest);
                _loadRequestsDb.RemoveLoadRequest(loadRequest);
            }
        }

        private void RemoveAssetsFromDb(List<AsyncOperationHandleWrapper> handleWrappers, ILoadRequest loadRequest)
        {
            foreach (var handleWrapper in handleWrappers)
            {
                foreach (var assetName in handleWrapper.AssetsNames)
                {
                    _assetsDb.RemoveAsset(loadRequest.AssetType, assetName);
                }

                if (handleWrapper.Handler.IsValid())
                    Addressables.Release(handleWrapper.Handler);
            }
        }
    }
}