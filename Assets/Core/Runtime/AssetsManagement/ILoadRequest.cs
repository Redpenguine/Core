using System;
using UnityEngine.AddressableAssets;

namespace Redpenguin.AssetsManagement
{
    public interface ILoadRequest
    {
        public Type AssetType { get; }
        public string[] LabelsOrAddress { get; }
        public Addressables.MergeMode MergeMode { get; }
    }
}