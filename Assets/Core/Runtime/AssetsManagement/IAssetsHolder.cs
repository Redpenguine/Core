using Cysharp.Threading.Tasks;

namespace Redpenguin.AssetsManagement
{
    public interface IAssetsHolder
    {
    }
    public interface IAssetsHolder<TLoadRequests> : IAssetsHolder where TLoadRequests : IAssetsLoadRequests, new()
    {
        UniTask LoadAssets();
        void ReleaseAssets();
    }

    public class AssetsHolder<TLoadRequests> : IAssetsHolder<TLoadRequests> where TLoadRequests : IAssetsLoadRequests, new()
    {
        protected TLoadRequests loadRequests = new();
        protected readonly IAssetsProvider assetsProvider;

        public AssetsHolder(IAssetsProvider assetsProvider)
        {
            this.assetsProvider = assetsProvider;
        }

        public async UniTask LoadAssets()
        {
            await assetsProvider.LoadAssets(this, loadRequests.LoadRequests);
        }

        public void ReleaseAssets()
        {
            assetsProvider.ReleaseAssets(this);
        }
    }

    public interface IAssetsLoadRequests
    {
        public ILoadRequest[] LoadRequests { get; }
    }

    
}