using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Redpenguin.AssetsManagement
{
    internal class AssetsLoader
    {
        private readonly AssetsDb _assetsDb;
        private readonly LoadRequestsDb _loadRequestsDb;
        private readonly AssetsHolderDb _assetsHolderDb;

        public AssetsLoader(LoadRequestsDb loadRequestsDb, AssetsHolderDb assetsHolderDb, AssetsDb assetsDb)
        {
            _assetsDb = assetsDb;
            _assetsHolderDb = assetsHolderDb;
            _loadRequestsDb = loadRequestsDb;
        }

        public async UniTask LoadAssets(IAssetsHolder assetsHolder, ILoadRequest loadRequest)
        {
            if (_loadRequestsDb.HasLoadRequest(loadRequest))
                return;
            AsyncOperationHandle<IList<IResourceLocation>> resourceLoadOperation = default;
            try
            {
                resourceLoadOperation = LoadResourceLocationsAsync(loadRequest);
                await resourceLoadOperation;
                if (resourceLoadOperation.Result.Count == 0)
                    return;

                var asyncOperationHandle = Addressables.LoadAssetsAsync<Object>(resourceLoadOperation.Result, null);
                await asyncOperationHandle;
                Debug.Log(
                    $"[AssetsLoader] LoadAssets {string.Join(",", asyncOperationHandle.Result.Select(x => x.name))}");
                var isAnyAssetsAdd = AddAnyAssetsToDb(loadRequest, ref asyncOperationHandle);
                if (!isAnyAssetsAdd)
                {
                    if (asyncOperationHandle.IsValid())
                        Addressables.Release(asyncOperationHandle);
                    return;
                }

                AddToDb(assetsHolder, loadRequest, asyncOperationHandle);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (resourceLoadOperation.IsValid())
                    Addressables.Release(resourceLoadOperation);
            }
        }

        private void AddToDb(IAssetsHolder assetsHolder, ILoadRequest loadRequest,
            AsyncOperationHandle<IList<Object>> asyncOperationHandle)
        {
            var asyncOperationHandleWrapper = new AsyncOperationHandleWrapper(asyncOperationHandle,
                asyncOperationHandle.Result.Select(x => x.name).ToArray());
            _loadRequestsDb.AddLoadRequest(loadRequest, asyncOperationHandleWrapper);
            _assetsHolderDb.AddAssetsHolder(assetsHolder, loadRequest);
        }

        private bool AddAnyAssetsToDb(ILoadRequest loadRequest,
            ref AsyncOperationHandle<IList<Object>> asyncOperationHandle)
        {
            var assetsCount = asyncOperationHandle.Result.Count;
            foreach (var asset in asyncOperationHandle.Result)
            {
                var isAddSuccess = _assetsDb.TryAddAsset(loadRequest.AssetType, asset);
                assetsCount -= isAddSuccess ? 1 : 0;
            }

            return assetsCount != asyncOperationHandle.Result.Count;
        }

        private AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(ILoadRequest loadRequest)
        {
            if (loadRequest.LabelsOrAddress.Length == 1)
            {
                return Addressables.LoadResourceLocationsAsync(loadRequest.LabelsOrAddress[0], loadRequest.AssetType);
            }

            return Addressables.LoadResourceLocationsAsync(new Labels(loadRequest.LabelsOrAddress),
                loadRequest.MergeMode, loadRequest.AssetType);
        }

        private class Labels : IEnumerable
        {
            private readonly string[] _people;

            public Labels(string[] pArray)
            {
                _people = new string[pArray.Length];

                for (int i = 0; i < pArray.Length; i++)
                {
                    _people[i] = pArray[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public LabelEnum GetEnumerator()
            {
                return new LabelEnum(_people);
            }
        }

        private class LabelEnum : IEnumerator
        {
            private readonly string[] people;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            private int _position = -1;

            public LabelEnum(string[] list)
            {
                people = list;
            }

            public bool MoveNext()
            {
                _position++;
                return (_position < people.Length);
            }

            public void Reset()
            {
                _position = -1;
            }

            object IEnumerator.Current => Current;

            private string Current
            {
                get
                {
                    try
                    {
                        return people[_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}