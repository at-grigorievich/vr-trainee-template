using System;
using ATG.Hands;
using ATG.VR.Extensions;
using UnityEngine;

namespace ATG.MVP.VR
{
    public enum HandType : byte
    {
        None = 0,
        Left = 1,
        Right = 2
    }
    
    [Serializable]
    public sealed class XrHandFactory
    {
        [SerializeField] private HandType handType;
        [SerializeField] private XrHandView view;
        [SerializeField] private InteractorManipuatorSetFactory manipulatorsFactory;
        [SerializeField] private XrHandAnimatorObserverCreator animatorObserverCreator;
        [SerializeField] private XrHandClampAmplitudeCreator clampAmplitudeCreator;
        
        public XrHandPresenter Create()
        {
            InteractorManipulatorsSet manipulators = manipulatorsFactory.Create();
            XrHandClampAmplitude clampAmplitude = clampAmplitudeCreator.Create();
            XrHandAnimatorObserver animatorObserver = animatorObserverCreator.Create(clampAmplitude);
            
            return new XrHandPresenter(handType, view, 
                manipulators,
                animatorObserver);
        }
    }
    
    public sealed class XrHandPresenter : Presenter<XrHandView>
    {
        private readonly InteractorManipulatorsSet _manipulators;
        
        private readonly XrHandClampAmplitude _clampAmplitude;
        private readonly XrHandAnimatorObserver _animatorObserver;
        
        public readonly HandType HandType;
        
        public XrHandPresenter(HandType handType, XrHandView view, 
            InteractorManipulatorsSet manipulators,
            XrHandAnimatorObserver animatorObserver) : base(view)
        {
            HandType = handType;
            
            _manipulators = manipulators;
            _animatorObserver = animatorObserver;
        }

        public override void Start()
        {
            base.Start();
            _animatorObserver.Start();
        }

        public override void Dispose()
        {
            base.Dispose();
            _animatorObserver.Dispose();
        }

        public override void Tick()
        {
            base.Tick();
            _animatorObserver.Tick();
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            _manipulators.SetActiveLogical(isActive);
        }

        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            _manipulators.SetActiveVisual(isActive);
        }

        public void ManipulatorSetActivateTotalByType(InteractionType interactionType, bool isActive)
        {
            _manipulators.SetActiveTotalByType(interactionType, isActive);
        }
        
        public bool TryGetManipulator(InteractionType interactionType, out InteractorManipulator manipulator)
        {
            return _manipulators.TryGetManipulatorByType(interactionType, out manipulator);
        }
    }
}