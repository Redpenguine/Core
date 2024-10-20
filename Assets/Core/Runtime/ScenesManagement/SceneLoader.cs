using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Redpenguin.ScenesManagement
{
    public interface ISceneLoader
    {
        public UniTask LoadSceneAsync(string sceneName, 
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default);
        
        public void LoadScene(string sceneName);
    }
    
    public class SceneLoader : ISceneLoader
    {
        private string _sceneName;
        private UniTask<(bool IsCanceled, SceneInstance Result)> _task;

        public async UniTask LoadSceneAsync(string sceneName, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            SetActiveSceneName();
            if(_sceneName == sceneName || _task.Status == UniTaskStatus.Pending)
                return;
            var sceneLoadOperation = Addressables.LoadSceneAsync(sceneName);
            _task = sceneLoadOperation
                .ToUniTask(progress: progress, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            var (isCanceled, sceneInstance) = await _task;
            if(!isCanceled) _sceneName = sceneInstance.Scene.name;
        }

        public void LoadScene(string sceneName)
        {
            SetActiveSceneName();
            if(_sceneName == sceneName || _task.Status == UniTaskStatus.Pending)
                return;
            SceneManager.LoadScene(sceneName);
            _sceneName = sceneName;
        }

        private void SetActiveSceneName()
        {
            _sceneName ??= SceneManager.GetActiveScene().name;
        }
    }
}