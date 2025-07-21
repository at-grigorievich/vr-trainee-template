using System;
using ATG.Activator;
using ATG.UI;
using UnityEngine;

namespace ATG.SceneManagement
{
    [Serializable]
    public sealed class ReloadSceneServiceFactory
    {
        [SerializeField] private RestartCurrentSceneView restartView;

        public ISceneReloader Create(ISceneManagement sceneManagement)
        {
            return new ReloadSceneService(sceneManagement, restartView);
        }
    }
    
    public sealed class ReloadSceneService : ISceneReloader
    {
        private readonly ISceneManagement _sceneManagement;
        private readonly SceneInfoData _initialSceneConfig;

        private readonly RestartCurrentSceneView _uiView;

        public ActiveStatus Status { get; set; }
        
        public ReloadSceneService(ISceneManagement sceneManagement, RestartCurrentSceneView uiView)
        {
            _uiView = uiView;
            _sceneManagement = sceneManagement;
        }
        
        public void SetActiveTotal(bool isActive)
        {
            SetActiveLogical(isActive);
            SetActiveVisual(isActive);
        }

        public void SetActiveVisual(bool isActive)
        {
            this.SetFlag(ActiveStatus.VISUAL_ACTIVE, isActive);
            
            if (isActive == true)
            {
                _uiView.Show(this, (Action)ReloadScene);
            }
            else
            {
                _uiView.Hide();
            }
        }

        public void SetActiveLogical(bool isActive)
        {
            this.SetFlag(ActiveStatus.LOGIC_ACTIVE, isActive);
        }
        
        public void ReloadScene()
        {
            _sceneManagement.RestartCurrentScene();
        }
    }
}