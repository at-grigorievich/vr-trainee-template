using System.Collections.Generic;
using JetBrains.Annotations;

namespace ATG.Services.Animator
{
    public class AnimatorWrapper: IAnimatorWrapper
    {
        private readonly UnityEngine.Animator _animator;
        private Dictionary<AnimatorTag, IAnimatorState> _statesByTag;
        
        public bool IsActive { get; private set; }
        
        [CanBeNull]
        public AnimatorEventDispatcher EventDispatcher { get; private set; }
        
        public AnimatorWrapper(UnityEngine.Animator animator, AnimatorStateSet data,
            AnimatorEventDispatcher eventDispatcher):this(animator, data)
        {
            EventDispatcher = eventDispatcher;
        }
        
        public AnimatorWrapper(UnityEngine.Animator animator, AnimatorStateSet data)
        {
            _animator = animator;
            _statesByTag = new Dictionary<AnimatorTag, IAnimatorState>(data.GetStatesByTag());
        }
        
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            _animator.enabled = IsActive;

            if (EventDispatcher != null)
            {
                EventDispatcher.SetActive(IsActive);
            }
        }

        public float GetStateLength(AnimatorTag tag)
        {
            return _statesByTag[tag].GetStateLength(_animator);
        }

        public void SelectState(AnimatorTag tag)
        {
            SetState(tag, null);
        }

        public void SetState(AnimatorTag tag, object value)
        {
            if(_statesByTag.ContainsKey(tag) == false)
                throw new KeyNotFoundException($"Animator tag {tag} not found");
            
            _statesByTag[tag].ChangeState(_animator, value);
        }

        public void SetStates(params StateData[] states)
        {
            foreach (var stateData in states)
            {
                SetState(stateData.Tag, stateData.Value);
            }
        }
    }
}