using System.Collections.Generic;

namespace Redpenguin.AssetsManagement
{
    internal class AssetsHolderDb
    {
        private readonly Dictionary<IAssetsHolder, List<ILoadRequest>> _assetsHolders = new();

        public void AddAssetsHolder(IAssetsHolder assetsHolder, ILoadRequest loadRequest)
        {
            if (!_assetsHolders.TryGetValue(assetsHolder, out var loadRequests))
            {
                loadRequests = new List<ILoadRequest>();
                _assetsHolders.Add(assetsHolder, loadRequests);
            }

            loadRequests.Add(loadRequest);
        }

        public bool HasAssetsHolder(IAssetsHolder assetsHolder)
        {
            return _assetsHolders.ContainsKey(assetsHolder);
        }

        public bool TryGetLoadRequests(IAssetsHolder assetsHolder, out List<ILoadRequest> loadRequests)
        {
            return _assetsHolders.TryGetValue(assetsHolder, out loadRequests);
        }

        public void RemoveAssetsHolder(IAssetsHolder assetsHolder)
        {
            _assetsHolders.Remove(assetsHolder);
        }
    }
}