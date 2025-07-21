using System;
using ATG.SceneManagement;
using ATG.Services.Scenario;
using VContainer.Unity;

namespace EntryPoints
{
    public class PlaySceneEntryPoint: IInitializable, IStartable, IDisposable
    {
        private readonly ISceneManagement _sceneManagement;
        private readonly StepByStepScenarioService _scenario;

        public PlaySceneEntryPoint(StepByStepScenarioService scenario)
        {
            _scenario = scenario;
        }
        
        public virtual void Initialize()
        {
            
        }

        public virtual void Start()
        {
            _scenario.Start();
        }

        public virtual void Dispose()
        {
            _scenario.Dispose();   
        }
    }
}