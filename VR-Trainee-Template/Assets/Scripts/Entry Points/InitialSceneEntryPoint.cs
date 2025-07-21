using System;
using ATG.MVP.VR;
using ATG.SceneManagement;
using ATG.Services.Admin;
using UnityEngine;
using VContainer.Unity;

namespace EntryPoints
{
    public class InitialSceneEntryPoint: IInitializable, IStartable, IDisposable
    {
        private readonly SceneInfoData _initialSceneInfo;
        private readonly SceneInfoData _nextSceneInfo;

        private readonly ISceneManagement _sceneManagement;
        private readonly IAdministrator _adminService;

        private readonly XrAgentPresenter _player;
        
        public InitialSceneEntryPoint(ISceneManagement sceneManagement, IAdministrator adminService, 
            XrAgentPresenter player,
            SceneInfoData initialSceneInfo, SceneInfoData nextSceneInfo)
        {
            _sceneManagement = sceneManagement;
            _adminService = adminService;
            
            _player = player;
            
            _initialSceneInfo = initialSceneInfo;
            _nextSceneInfo = nextSceneInfo;
        }
        
        public void Initialize()
        {
            _sceneManagement.SetupLoadedScene(_initialSceneInfo);
            _player.SetActiveTotal(false);
        }
        
        public void Start()
        {
            _adminService.SetActiveLogical(true);
            _adminService.SetActiveVisual(false);

            if (_nextSceneInfo != null)
            {
                _sceneManagement.LoadBySceneInfoAsync(_nextSceneInfo);
            }
        }

        public void Dispose()
        {
            _adminService.Dispose();
        }
    }
}