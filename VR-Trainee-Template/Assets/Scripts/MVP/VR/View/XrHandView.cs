using UnityEngine;

namespace ATG.MVP.VR
{
    public class XrHandView : View
    {
        [SerializeField] Renderer[] renderers;
         
        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);

            foreach (var renderer1 in renderers)
            {
                renderer1.enabled = isActive;
            }
        }
    }
}