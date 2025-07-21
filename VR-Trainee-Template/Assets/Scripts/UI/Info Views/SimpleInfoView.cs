using UnityEngine;

namespace ATG.UI
{
    public sealed class SimpleInfoView: InfoView
    {
        [SerializeField] private bool isHideOnAwake;

        private new void Awake()
        {
            base.Awake();
            
            SetActiveTotal(!isHideOnAwake);
        }

        public void Show()
        {
            base.Show(this, null);
            
            if (useFadeAnimation == true)
            {
                Fade();
            }
            else
            {
                canvasGroup.alpha = defaultAlpha;
            }
        }
    }
}