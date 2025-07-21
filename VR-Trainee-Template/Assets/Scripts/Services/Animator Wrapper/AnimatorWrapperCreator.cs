using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ATG.Services.Animator
{
    [Serializable]
    public class AnimatorWrapperCreator
    {
        [SerializeField] private AnimatorStateSet animatorSet;
        [SerializeField] private UnityEngine.Animator animator;
        
        [SerializeField] private bool withEventDispatcher;
        [SerializeField, ShowIf("withEventDispatcher")]
        private AnimatorEventDispatcher animatorEventDispatcher;
        
        public IAnimatorWrapper Create()
        {
            if (withEventDispatcher == true)
                return new AnimatorWrapper(animator, animatorSet, animatorEventDispatcher);
            
            return new AnimatorWrapper(animator, animatorSet);
        }
        
        public IAnimatorWrapper Create(UnityEngine.Animator overrideAnimator)
        {
            if (withEventDispatcher == true)
                return new AnimatorWrapper(animator, animatorSet, animatorEventDispatcher);
            
            return new AnimatorWrapper(overrideAnimator, animatorSet);
        }
    }
}