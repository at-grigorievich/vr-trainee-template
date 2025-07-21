using ATG.SceneManagement;
using ATG.Services.VRDevice;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class ProjectScope: LifetimeScope
    {
        [SerializeField] private DeviceDetectorCreator deviceDetectorCreator;
        
        protected override void Configure(IContainerBuilder builder)
        {
            deviceDetectorCreator.Create(builder);
            
            builder.Register<SceneManagement>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}