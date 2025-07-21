using System;
using System.Collections.Generic;
using System.Linq;
using ATG.Activator;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ATG.VR.Extensions
{
    [Serializable]
    public sealed class InteractorManipuatorSetFactory
    {
        [SerializeField] private InteractorManipulatorFactory[] interactorFactories;

        public InteractorManipulatorsSet Create() =>
            new(interactorFactories.Select(f => f.Create()));
    }
    
    public sealed class InteractorManipulatorsSet: ActivateObject
    {
        private readonly Dictionary<InteractionType, InteractorManipulator> _manipulators;

        public InteractorManipulatorsSet(IEnumerable<InteractorManipulator> manipulators)
        {
            _manipulators = new Dictionary<InteractionType, InteractorManipulator>(
                manipulators.Select(m => 
                    new KeyValuePair<InteractionType, InteractorManipulator>(m.InteractionType, m)));
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            ForAll(manipulator => manipulator.SetActiveLogical(isActive));
        }

        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            ForAll(manipulator => manipulator.SetActiveVisual(isActive));
        }

        public void SetActiveTotalByType(InteractionType type, bool isActive)
        {
            if(TryGetManipulatorByType(type, out InteractorManipulator manipulator) == false) return;
            manipulator.SetActiveTotal(isActive);
        }

        public bool TryGetManipulatorByType(InteractionType type, out InteractorManipulator manipulator)
        {
            if (_manipulators.TryGetValue(type, out var result) == false)
            {
                Debug.LogWarning($"InteractionManipulator with type {type} not found");
                manipulator = null;
                return false;
            }
            
            manipulator = result;
            return true;
        }

        public bool TryGetHoverManipulatorByType(InteractionType type, out IXRHoverInteractor hover)
        {
            if (TryGetManipulatorByType(type, out InteractorManipulator manipulator) == false)
            {
                hover = null;
                return false;
            }

            hover = manipulator.Hover;
            return true;
        }
        
        public bool TryGetSelectManipulatorByType(InteractionType type, out IXRSelectInteractor select)
        {
            if (TryGetManipulatorByType(type, out InteractorManipulator manipulator) == false)
            {
                select = null;
                return false;
            }

            select = manipulator.Select;
            return true;
        }

        private void ForAll(Action<InteractorManipulator> action)
        {
            foreach (var keyValuePair in _manipulators)
            {
                InteractorManipulator manipulator = keyValuePair.Value;
                
                action?.Invoke(manipulator);
            }
        }
    }
}