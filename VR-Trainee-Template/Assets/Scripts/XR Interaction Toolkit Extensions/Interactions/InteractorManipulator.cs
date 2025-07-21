using System;
using ATG.Activator;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ATG.VR.Extensions
{
    [Serializable]
    public sealed class InteractorManipulatorFactory
    {
        [SerializeField] private InteractionType interactionType;
        [SerializeField] private XRBaseInteractor interactor;
        [SerializeField] private bool withVisual;
        [SerializeField, ShowIf("withVisual")] private MonoBehaviour visual;
        
        public InteractorManipulator Create() => new (interactionType, interactor, 
            withVisual == true ? visual : null);
    }
    
    public class InteractorManipulator: ActivateObject
    {
        private readonly InteractionType _interactionType;
        private readonly XRBaseInteractor _interactor;
        private readonly MonoBehaviour _visual;
        
        public InteractionType InteractionType => _interactionType;
        public IXRHoverInteractor Hover => _interactor;
        public IXRSelectInteractor Select => _interactor;

        public InteractorManipulator(InteractionType type, XRBaseInteractor interactor, MonoBehaviour visual = null)
        {
            _interactionType = type;
            _interactor = interactor;
            _visual = visual;
        }
        
        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            _interactor.enabled = isActive;
        }
        
        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            if (_visual != null)
            {
                _visual.enabled = isActive;
            }
        }
    }
}