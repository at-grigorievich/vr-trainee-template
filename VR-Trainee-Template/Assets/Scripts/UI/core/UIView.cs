using UnityEngine;

namespace ATG.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIView : UIElement
    {
        protected Canvas _canvas;

        public abstract UIElementType ViewType { get; }

        protected void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            
            if(_canvas == null)
                _canvas = GetComponent<Canvas>();

            _canvas.enabled = isActive;
        }

        public void ChangeLayerIndex(int index) => _canvas.sortingLayerID = index;
    }
}