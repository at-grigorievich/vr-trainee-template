using System;
using ATG.Activator;
using ATG.SceneManagement;
using ATG.UI;
using UnityEngine;

namespace ATG.Services.Quiz
{
    [Serializable]
    public sealed class QuizSceneLoaderFactory
    {
        [SerializeField] private GoToQuizView goToQuizView;
        [SerializeField] private SceneInfoData quizScene;

        public QuizSceneLoader Create(ISceneManagement sceneManagement)
        {
            return new QuizSceneLoader(sceneManagement, quizScene, goToQuizView);
        }
    }
    
    public sealed class QuizSceneLoader: SceneLoader, IActivateable
    {
        private readonly GoToQuizView _goToQuizView;

        private readonly SceneInfoData _quizScene;

        public ActiveStatus Status { get; set; }
        
        public QuizSceneLoader(ISceneManagement sceneManager, SceneInfoData quizScene, GoToQuizView goToQuizView)
            : base(sceneManager)
        {
            _quizScene = quizScene;
            _goToQuizView = goToQuizView;
        }

        public void SetActiveTotal(bool isActive)
        {
            SetActiveVisual(isActive);
            SetActiveLogical(isActive);
        }

        public void SetActiveVisual(bool isActive)
        {
            this.SetFlag(ActiveStatus.VISUAL_ACTIVE, isActive);
            
            if (isActive == true)
            {
                ActivateSceneReloading();
            }
            else
            {
                DeactivateSceneReloading();
            }
        }

        public void SetActiveLogical(bool isActive)
        {
            this.SetFlag(ActiveStatus.LOGIC_ACTIVE, isActive);
        }

        protected override SceneInfoData GetSceneInfo()
        {
            return _quizScene;
        }

        private void ActivateSceneReloading()
        {
            _goToQuizView.Show(this, (Action)Load);
        }

        private void DeactivateSceneReloading()
        {
            _goToQuizView.Hide();
        }
    }
}