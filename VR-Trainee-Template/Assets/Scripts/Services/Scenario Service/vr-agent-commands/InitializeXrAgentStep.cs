using ATG.MVP.VR;
using UnityEngine;
using VContainer;

namespace ATG.Services.Scenario
{
    public class InitializeXrAgentStep: ScenarioCommand
    {
        [SerializeField] private Transform spawnPoint;
        
        private XrAgentPresenter _player;
        
        public override void Resolve(IObjectResolver resolver)
        {
            _player = resolver.Resolve<XrAgentPresenter>();
        }

        public override void Execute()
        {
            base.Execute();
            
            _player.Place(spawnPoint);
            _player.SetActiveTotal(true);
            
            OnComplete(true);
        }
    }
}