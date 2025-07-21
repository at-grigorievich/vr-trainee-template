using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ATG.SceneManagement
{
    public class SceneManagement: ISceneManagement, IDisposable
    {
        private CancellationTokenSource _cts;
        
        public SceneInfoData CurrentScene { get; private set; }
        
        public async UniTask LoadBySceneInfoAsync(SceneInfoData sceneInfo)
        {
            Cancel();
            
            int? sceneIndex = sceneInfo.GetBuildSettingsIndex();

            if (sceneIndex.HasValue == false) return;
            
            _cts = new CancellationTokenSource();

            if (await UnloadCurrentScene(_cts.Token) == false)
            {
                Debug.LogWarning($"Can't allow to unload scene: {CurrentScene?.SceneName}");
                
                if(sceneInfo.LoadAdditive == false) return;
            }
            
            if (sceneInfo.LoadAdditive == true)
            {
                await LoadBySceneInfoAdditiveAsync(sceneIndex.Value, _cts.Token);
            }
            else
            {
                await LoadBySceneInfoSingleAsync(sceneIndex.Value, _cts.Token);
            }
            
            CurrentScene = sceneInfo;
            UpdateCurrentSceneSkybox();
        }

        public void SetupLoadedScene(SceneInfoData sceneInfo)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (sceneInfo.SceneName != currentSceneName)
                throw new Exception($"Invalid scene info config! Loaded scene name is {currentSceneName}");
            
            CurrentScene = sceneInfo;
            UpdateCurrentSceneSkybox();
        }

        public void RestartCurrentScene()
        {
            if(CurrentScene == null) return;
            LoadBySceneInfoAsync(CurrentScene).Forget();
        }

        public void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
        
        public void Dispose()
        {
            CurrentScene = null;
            
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTask<bool> UnloadCurrentScene(CancellationToken token)
        {
            if(CurrentScene == null) return false;
            
            if(CurrentScene.AllowUnload == false) return false;

            AsyncOperation operation = SceneManager.UnloadSceneAsync(CurrentScene.GetBuildSettingsIndex()!.Value);
            await operation.ToUniTask(cancellationToken: token);
            
            CurrentScene = null;

            return true;
        }
        
        private async UniTask LoadBySceneInfoSingleAsync(int sceneIndex, CancellationToken token)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            await operation.ToUniTask(cancellationToken: token);
        }

        private async UniTask LoadBySceneInfoAdditiveAsync(int sceneIndex, CancellationToken token)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            await asyncLoad.ToUniTask(cancellationToken: token);
        }

        private void UpdateCurrentSceneSkybox()
        {
            if(CurrentScene == null) return;
            if(CurrentScene.Skybox == null) return;
            
            RenderSettings.skybox = CurrentScene.Skybox;
            DynamicGI.UpdateEnvironment();
        }
    }
}