using UnityEngine;

namespace ATG.Services.Outlines
{
    public sealed class SingleOutlineWithAnimation: BaseOutlineView
    {
        [Space(5)]
        [SerializeField] private OutlineViewParameters config;

        private new void Awake()
        {
            base.Awake();
            SwitchOutlineIndex(config.GetIndex());
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            
            if (isActive == true)
            {
                _outlineService.AddAnimation(config, this);
            }
            else
            {
                Dispose();
            }
        }
        public override void Dispose()
        {
            _outlineService.StopAnimation(config, this);
        }
    }
}