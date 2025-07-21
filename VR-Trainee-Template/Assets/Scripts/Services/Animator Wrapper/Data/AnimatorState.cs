using System;
using UnityEngine;

namespace ATG.Services.Animator
{
    [Serializable]
    public class CrossFadeOption
    {
        [field: SerializeField, Range(0f, 1f)] public float CrossFadeDuration;
        [field: SerializeField] public AnimatorLayer AnimatorLayer = AnimatorLayer.BASE;
    }
    
    public interface IAnimatorState
    {
        AnimatorTag Tag { get; }
        string StateName { get; }
        
        int StateIndex { get; }
        
        void ChangeState(UnityEngine.Animator animator, object value);
        
        public float GetStateLength(UnityEngine.Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
        
            foreach (var clip in clips)
            {
                int hash = UnityEngine.Animator.StringToHash(clip.name);
                if(StateIndex == hash) return clip.length;
            }

            throw new System.ArgumentException("Invalid state");
        }
    }
    
    [Serializable]
    public sealed class BoolAnimatorState : IAnimatorState
    {
        [field: SerializeField] public AnimatorTag Tag { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        public int StateIndex => UnityEngine.Animator.StringToHash(StateName);

        public void ChangeState(UnityEngine.Animator animator, object value)
        {
            if(value is not bool b)
                throw new System.ArgumentException("Value is not bool");
            
            animator.SetBool(StateIndex, b);
        }
    }
    
    [Serializable]
    public sealed class FloatAnimatorState : IAnimatorState
    {
        [field: SerializeField] public AnimatorTag Tag { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        public int StateIndex => UnityEngine.Animator.StringToHash(StateName);
        
        public void ChangeState(UnityEngine.Animator animator, object value)
        {
            if(value is not float f)
                throw new System.ArgumentException("Value is not float");
            
            animator.SetFloat(StateIndex, f);
        }
    }

    [Serializable]
    public sealed class IntAnimatorState : IAnimatorState
    {
        [field: SerializeField] public AnimatorTag Tag { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        public int StateIndex => UnityEngine.Animator.StringToHash(StateName);
        
        public void ChangeState(UnityEngine.Animator animator, object value)
        {
            if(value is not int i)
                throw new System.ArgumentException("Value is not int");
            
            animator.SetInteger(StateIndex, i);
        }
    }

    [Serializable]
    public sealed class TriggerAnimatorState : IAnimatorState
    {
        [field: SerializeField] public AnimatorTag Tag { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        public int StateIndex => UnityEngine.Animator.StringToHash(StateName);
        
        public void ChangeState(UnityEngine.Animator animator, object value)
        {
            animator.SetTrigger(StateIndex);
        }
    }

    [Serializable]
    public sealed class CrossfadeAnimatorState : IAnimatorState
    {
        [SerializeField] private bool ignoreTimeOffset = true;
        [field: SerializeField] public AnimatorTag Tag { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        public int StateIndex => UnityEngine.Animator.StringToHash(StateName);
        
        [SerializeField] private CrossFadeOption option;
        
        public void ChangeState(UnityEngine.Animator animator, object value)
        {
            if (ignoreTimeOffset == true)
            {
                animator.CrossFade(StateIndex, option.CrossFadeDuration, (int)option.AnimatorLayer);
            }
            else
            {
                animator.CrossFade(StateIndex, option.CrossFadeDuration, (int)option.AnimatorLayer, 0f);
            }
        }
    }
}