using System;
using ATG.Services.Animator;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace ATG.Hands
{
    [Serializable]
    public sealed class XrHandAnimatorObserverCreator
    {
        [SerializeField] private AnimatorWrapperCreator animatorCreator;
        [SerializeField] private InputActionReference triggerAction;
        [SerializeField] private InputActionReference gripAction;
        
        public XrHandAnimatorObserver Create(XrHandClampAmplitude clampAmplitude)
        {
            IAnimatorWrapper animator = animatorCreator.Create();
            return new XrHandAnimatorObserver(animator, clampAmplitude, gripAction, triggerAction);
        }
    }
    
    public class XrHandAnimatorObserver: IStartable, ITickable, IDisposable
    {
        private readonly IAnimatorWrapper _animator;

        private readonly InputActionReference _gripAction;
        private readonly InputActionReference _triggerAction;

        private readonly XrHandClampAmplitude _clampAmplitude;

        private bool _allowTick;
        
        public XrHandAnimatorObserver(IAnimatorWrapper animator, XrHandClampAmplitude clampAmplitude,
            InputActionReference gripAction, InputActionReference triggerAction)
        {
            _animator = animator;
            _clampAmplitude = clampAmplitude;
            
            _gripAction = gripAction;
            _triggerAction = triggerAction;
        }

        public void Start()
        {
            _animator.SetActive(true);
            _allowTick = true;
            
            _gripAction.action.Enable();
            _triggerAction.action.Enable();
            
            _clampAmplitude.ResetAll();
        }

        public void Tick()
        {
            if(_allowTick == false) return;
            
            float gripValue = _gripAction.action.ReadValue<float>();
            float triggerValue = _triggerAction.action.ReadValue<float>();
            
            gripValue = _clampAmplitude.GetClampedGripAmplitude(gripValue);
            triggerValue = _clampAmplitude.GetClampedTriggerAmplitude(triggerValue);
            
            AnimateGripAction(gripValue);
            AnimateTriggerAction(triggerValue);
        }
        
        public void Dispose()
        {
            _gripAction.action.Disable();
            _triggerAction.action.Disable();
            _allowTick = false;
        }
        
        private void AnimateGripAction(float gripValue) =>
            _animator.SetState(AnimatorTag.Grip, gripValue);

        private void AnimateTriggerAction(float triggerValue) =>
            _animator.SetState(AnimatorTag.Trigger, triggerValue);
    }
}