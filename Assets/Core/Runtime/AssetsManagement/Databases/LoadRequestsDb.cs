using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Redpenguin.AssetsManagement
{
    internal class AsyncOperationHandleWrapper
    {
        public AsyncOperationHandle Handler { get; }
        public string[] AssetsNames { get; }

        public AsyncOperationHandleWrapper(AsyncOperationHandle handler, string[] assetsNames)
        {
            Handler = handler;
            AssetsNames = assetsNames;
        }
    }
    internal class LoadRequestsDb
    {
        private readonly Dictionary<ILoadRequest, List<AsyncOperationHandleWrapper>> _loadRequests = new();
        
        public void AddLoadRequest(ILoadRequest loadRequest, AsyncOperationHandleWrapper asyncOperationHandle)
        {
            if (!_loadRequests.TryGetValue(loadRequest, out var asyncOperationHandles))
            {
                asyncOperationHandles = new List<AsyncOperationHandleWrapper>();
                _loadRequests.Add(loadRequest, asyncOperationHandles);
            }

            asyncOperationHandles.Add(asyncOperationHandle);
        }
        
        public bool HasLoadRequest(ILoadRequest loadRequest)
        {
            return _loadRequests.ContainsKey(loadRequest);
        }
        
        public bool TryGetLoadRequests(ILoadRequest loadRequest, out List<AsyncOperationHandleWrapper> asyncOperationHandles)
        {
            return _loadRequests.TryGetValue(loadRequest, out asyncOperationHandles);
        }
        
        public void RemoveLoadRequest(ILoadRequest loadRequest)
        {
            _loadRequests.Remove(loadRequest);
        }
    }
}