using ATG.Services.Scenario;
using EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class PlaySceneScope: LifetimeScope
    {
        [SerializeField] private StepByStepScenarioCreator scenarioCreator;

        protected override void Configure(IContainerBuilder builder)
        {
            scenarioCreator.Create(builder);
            
            RegisterEntryPoint(builder);
        }

        protected virtual void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlaySceneEntryPoint>();
        }
    }
}