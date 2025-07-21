using ATG.MVP.VR;
using ATG.SceneManagement;
using ATG.Services.Admin;
using EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class InitialSceneScope: LifetimeScope
    {
        [SerializeField] private SceneInfoData initialScene;
        [SerializeField] private SceneInfoData nextScene;
        [Space(10)]
        [SerializeField] private AdminServiceCreator adminServiceCreator;
        [SerializeField] private XrAgentCreator agentCreator;
        
        protected override void Configure(IContainerBuilder builder)
        {
            adminServiceCreator.Create(builder);
            agentCreator.Create(builder);

            builder.RegisterEntryPoint<InitialSceneEntryPoint>()
                .WithParameter("initialSceneInfo", initialScene)
                .WithParameter("nextSceneInfo", nextScene);
        }
    }
}