using System;
using Cysharp.Threading.Tasks;

namespace ATG.SceneManagement
{
    public interface ISceneManagement
    {
        public SceneInfoData CurrentScene { get; }
        
        UniTask LoadBySceneInfoAsync(SceneInfoData sceneInfo);

        void SetupLoadedScene(SceneInfoData sceneInfo);
        void RestartCurrentScene();
        void Cancel();
    }
}