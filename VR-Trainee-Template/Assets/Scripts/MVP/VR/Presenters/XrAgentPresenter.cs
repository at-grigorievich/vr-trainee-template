using System;
using System.Collections.Generic;
using System.Linq;
using ATG.VR.Extensions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using VContainer;

namespace ATG.MVP.VR
{
    [Serializable]
    public sealed class XrAgentCreator
    {
        [SerializeField] private XrAgentView view;
        [Space(10)]
        [SerializeField] private XrHandFactory leftHandFactory;
        [SerializeField] private XrHandFactory rightHandFactory;
        [Space(10)]
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private ContinuousMoveProvider continuousMoveProvider;
        
        public void Create(IContainerBuilder builder)
        {
            XrHandPresenter leftHand = leftHandFactory.Create();
            XrHandPresenter rightHand = rightHandFactory.Create();

            builder.Register<XrAgentPresenter>(Lifetime.Singleton)
                .WithParameter<XrAgentView>(view)
                .WithParameter(teleportationProvider)
                .WithParameter(continuousMoveProvider)
                .WithParameter<IEnumerable<XrHandPresenter>>(new[] { leftHand, rightHand })
                .AsSelf().AsImplementedInterfaces();
        }
    }
    
    public sealed class XrAgentPresenter: Presenter<XrAgentView>
    {
        private readonly Dictionary<HandType, XrHandPresenter> _hands;

        private readonly TeleportationProvider _teleportationProvider;
        private readonly ContinuousMoveProvider _continuousMoveProvider;
            
        public XrAgentPresenter(XrAgentView view, 
            TeleportationProvider teleportation, ContinuousMoveProvider continuousMoveProvider,
            IEnumerable<XrHandPresenter> hands) : base(view)
        {
            _hands = new Dictionary<HandType, XrHandPresenter>(hands
                .Select(hand => new KeyValuePair<HandType, XrHandPresenter>(hand.HandType, hand)));

            _teleportationProvider = teleportation;
            _continuousMoveProvider = continuousMoveProvider;
        }

        public override void Start()
        {
            base.Start();
            _hands[HandType.Right].Start();
            _hands[HandType.Left].Start();
        }

        public override void Tick()
        {
            base.Tick();
            _hands[HandType.Right].Tick();
            _hands[HandType.Left].Tick();
        }

        public override void Dispose()
        {
            base.Dispose();
            _hands[HandType.Right].Dispose();
            _hands[HandType.Left].Dispose();
        }

        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            _view.SetActiveVisual(isActive);
            
            _hands[HandType.Left].SetActiveVisual(isActive);
            _hands[HandType.Right].SetActiveVisual(isActive);
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            _view.SetActiveLogical(isActive);
            
            _hands[HandType.Left].SetActiveLogical(isActive);
            _hands[HandType.Right].SetActiveLogical(isActive);
            
            TeleportationSetActive(isActive);
            ContinuousMoveSetActive(isActive);
        }

        public void Place(Transform placePoint)
        {
            _view.Place(placePoint.position, placePoint.rotation);
            _view.ResetOffset();
        }

        public void TeleportationSetActive(bool isActive)
        {
            _teleportationProvider.enabled = isActive;
        }

        public void ContinuousMoveSetActive(bool isActive)
        {
            _continuousMoveProvider.enabled = isActive;
        }

        public void SetTotalActiveManipulators(InteractionType interactionType, bool isActive)
        {
            _hands[HandType.Left].ManipulatorSetActivateTotalByType(interactionType, isActive);
            _hands[HandType.Right].ManipulatorSetActivateTotalByType(interactionType, isActive);
        }
        
        public bool TryGetManipulator(HandType handType, InteractionType interactionType,
            out InteractorManipulator manipulator)
        {
            return _hands[handType].TryGetManipulator(interactionType, out manipulator);
        }
    }
}