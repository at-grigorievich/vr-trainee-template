using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ATG.Services.Animator
{
    [CreateAssetMenu(fileName = "new-animator-set", menuName = "Configs/New Animator Set", order = 0)]
    public class AnimatorStateSet : ScriptableObject
    {
        [SerializeReference] public IAnimatorState[] States;
        [SerializeField] private Dictionary<AnimatorTag, float> _statesLength;
        
        public IEnumerable<KeyValuePair<AnimatorTag, IAnimatorState>> GetStatesByTag() =>
            States.Select(state => new KeyValuePair<AnimatorTag, IAnimatorState>(state.Tag, state));
    }
}
