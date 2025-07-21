using System;
using System.Linq;
using ATG.Command;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ATG.Services.Scenario
{
    [Serializable]
    public sealed class StepByStepScenarioCreator
    {
        [SerializeField] private ScenarioCommand[] commands;

        public void Create(IContainerBuilder builder)
        {
            
            builder.Register<StepByStepScenarioService>(Lifetime.Singleton)
                .WithParameter(commands)
                .AsSelf().AsImplementedInterfaces();
        }
    }
    
    public sealed class StepByStepScenarioService: IDisposable
    {
        private readonly CommandInvoker _commandInvoker;
        
        public event Action<bool> OnCompleted
        {
            add => _commandInvoker.OnCompleted += value;
            remove => _commandInvoker.OnCompleted -= value;
        }
        
        public StepByStepScenarioService(IObjectResolver resolver, ScenarioCommand[] commands)
        {
            foreach (var scenarioCommand in commands)
            {
                scenarioCommand.Resolve(resolver);
            }

            _commandInvoker = new CommandInvoker(false, commands.Cast<ICommand>().ToArray());
        }
        
        public void Start()
        {
            _commandInvoker.Execute();
        }

        public void Dispose()
        {
            _commandInvoker.Dispose();
        }
    }
}