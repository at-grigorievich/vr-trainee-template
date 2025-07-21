using System;
using UnityEngine;

namespace ATG.Services.Outlines
{
    public static class OutlineViewExtensions
    {
        public static int AvailableMaxIndex = 1;
        
        public static int GetIndex(this OutlineViewParameters config)
        {
            return config.AnimationType switch
            {
                OutlineAnimationType.BlinkOne => 0,
                OutlineAnimationType.BlinkTwo => 1,
                _ => 0
            };
        }
    }
    
    [CreateAssetMenu(fileName = "new outline parameters", menuName = "configs/outlines/new outline parameters", order = 1)]
    public sealed class OutlineViewParameters: ScriptableObject
    {
        [field: SerializeField] public OutlineAnimationType AnimationType { get; private set; } = OutlineAnimationType.BlinkOne;
        [field: SerializeField] public Color InitialColor { get; private set; } = Color.white;
        [field: SerializeField] public Color FinalColor { get; private set; } = Color.white;
        [field: SerializeField] public float Duration { get; private set; } = 1.2f;
    }
}