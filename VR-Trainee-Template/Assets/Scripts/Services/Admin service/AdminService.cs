using System;
using ATG.Activator;
using ATG.SceneManagement;
using ATG.Services.Calibration;
using ATG.Services.Quiz;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace ATG.Services.Admin
{
    [Serializable]
    public class AdminServiceCreator
    {
        [SerializeField] private CalibrationServiceFactory calibrationServiceFactory;
        [SerializeField] private QuizSceneLoaderFactory quizLoaderFactory;
        [SerializeField] private ReloadSceneServiceFactory reloadSceneServiceFactory;
        [Space(10)]
        [SerializeField] private Canvas adminPanelCanvas;
        [SerializeField] private InputActionReference activateAdminInput;

        public void Create(IContainerBuilder builder)
        {
            builder.Register<IAdministrator, AdminService>(Lifetime.Singleton)
                .WithParameter<CalibrationServiceFactory>(calibrationServiceFactory)
                .WithParameter<ReloadSceneServiceFactory>(reloadSceneServiceFactory)
                .WithParameter<QuizSceneLoaderFactory>(quizLoaderFactory)
                .WithParameter<Canvas>(adminPanelCanvas)
                .WithParameter<InputActionReference>(activateAdminInput);
        }
    }
    
    /// <summary>
    /// Фасад для работы с сервисами: Калибровка высоты, Перемещение сразу к квизу, Перезагрузка текущей сцены
    /// + контроллирует UI отображение
    /// </summary>
    public class AdminService : IAdministrator
    {
        private readonly InputActionReference _adminEnteredInput;

        private readonly Canvas _adminCanvas;

        private readonly ICallibrationService _calibrationService;
        private readonly ISceneReloader _sceneReloadService;
        private readonly QuizSceneLoader _quizSceneLoader;
        
        public ActiveStatus Status { get; set; }
        
        public AdminService(CalibrationServiceFactory calibrationServiceFactory, 
                            ReloadSceneServiceFactory reloadSceneServiceFactory, 
                            QuizSceneLoaderFactory quizTeleporteableFactory, 
                            Canvas adminCanvas,
                            InputActionReference adminEnteredInput,
                            ISceneManagement sceneManagement)
        {
            _calibrationService = calibrationServiceFactory.Create();
            _sceneReloadService = reloadSceneServiceFactory.Create(sceneManagement);
            _quizSceneLoader = quizTeleporteableFactory.Create(sceneManagement);

            _adminEnteredInput = adminEnteredInput;

            _adminCanvas = adminCanvas;
        }
        
        public void SetActiveTotal(bool isActive)
        {
            SetActiveLogical(isActive);
            SetActiveVisual(isActive);
        }

        public void SetActiveVisual(bool isActive)
        {
            this.SetFlag(ActiveStatus.VISUAL_ACTIVE, isActive);

            _adminCanvas.enabled = isActive;
            
            _calibrationService.SetActiveTotal(isActive);
            _sceneReloadService.SetActiveTotal(isActive);
            _quizSceneLoader.SetActiveTotal(isActive);
        }

        public void SetActiveLogical(bool isActive)
        {
            this.SetFlag(ActiveStatus.LOGIC_ACTIVE, isActive);
            
            if(isActive == true)
            {
                _adminEnteredInput.action.performed += OnActivate;
            }
            else
            {
                _adminEnteredInput.action.performed -= OnActivate;
            }
        }

        private void OnActivate(InputAction.CallbackContext context)
        {
            SetActiveVisual(!this.IsVisibleActive());
        }

        public void Dispose()
        {
            _adminEnteredInput.action.performed -= OnActivate;
        }
    }
}