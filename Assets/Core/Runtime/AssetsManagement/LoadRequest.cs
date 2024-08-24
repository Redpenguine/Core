using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Redpenguin.AssetsManagement
{
    public struct LoadRequest<T> : ILoadRequest, IEqualityComparer<LoadRequest<T>> where T : Object
    {
        private LoadRequest(string[] labelsOrAddress,
            Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection)
        {
            LabelsOrAddress = labelsOrAddress;
            MergeMode = mergeMode;
        }

        public Type AssetType => typeof(T);

        public string[] LabelsOrAddress { get; }

        public Addressables.MergeMode MergeMode { get; }

        public bool Equals(LoadRequest<T> x, LoadRequest<T> y)
        {
            return x.LabelsOrAddress.SequenceEqual(y.LabelsOrAddress) && x.MergeMode == y.MergeMode;
        }

        public int GetHashCode(LoadRequest<T> obj)
        {
            return HashCode.Combine(obj.LabelsOrAddress, (int) obj.MergeMode);
        }
        
        public static LoadRequest<T> Create(string labelOrAddress)
        {
            return new LoadRequest<T>(new[] {labelOrAddress});
        }
        
        public static LoadRequest<T> Create(string label1, string label2,
            Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection)
        {
            return new LoadRequest<T>(new[] {label1, label2}, mergeMode);
        }

        public static LoadRequest<T> Create(string label1, string label2, string label3,
            Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection)
        {
            return new LoadRequest<T>(new[] {label1, label2, label3}, mergeMode);
        }
    }
}