using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Redpenguin.ScenesManagement
{
    public class LoadSceneTest : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        [ContextMenu("LoadTestScene")]
        private async void LoadTestScene()
        {
            await _sceneLoader.LoadSceneAsync("TestScene").ContinueWith(() => Debug.Log("Scene loaded"));
        }
        
        [ContextMenu("LoadTestScene1")]
        private async void LoadTestScene1()
        {
            await _sceneLoader.LoadSceneAsync("TestScene1");
        }
    }
}