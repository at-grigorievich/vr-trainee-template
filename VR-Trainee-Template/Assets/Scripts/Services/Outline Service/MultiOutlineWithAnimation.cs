using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ATG.Services.Outlines
{
    public sealed class MultiOutlineWithAnimation: BaseOutlineView
    {
        [Space(5)]
        [SerializeField] private OutlineViewParameters[] configs;
        
        private OutlineViewParameters _currentConfig;

        private new void Awake()
        {
            base.Awake();
            SwitchOutlineIndex(0);
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);

            if (isActive == true)
            {
                _outlineService.AddAnimation(_currentConfig, this);
            }
            else
            {
                Dispose();
            }
        }
        
        public override void SwitchOutlineIndex(int index)
        {
            if (index < 0 || index > OutlineViewExtensions.AvailableMaxIndex)
                throw new ArgumentOutOfRangeException($"Try to set up outline index out of range: {index}, max index: {OutlineViewExtensions.AvailableMaxIndex}. GameObject: {gameObject.name}");
            
            Dispose();
            _currentConfig = configs[index];
            base.SwitchOutlineIndex(_currentConfig.GetIndex());
        }
        
        public override void Dispose()
        {
            if(_currentConfig == null) return;
            _outlineService.StopAnimation(_currentConfig, this);
        }

        [Button("Switch outline index")]
        public void SwitchOutlineIndexDebug(int index)
        {
            SwitchOutlineIndex(index);
        }
    }
}