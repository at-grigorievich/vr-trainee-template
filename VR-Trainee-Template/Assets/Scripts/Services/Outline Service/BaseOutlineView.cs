using System;
using ATG.Activator;
using cakeslice;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ATG.Services.Outlines
{
    public abstract class BaseOutlineView: ActivateBehaviour, IDisposable
    {
        [SerializeField] private bool showOnAwake = false;
        [SerializeField] private bool addOutlineToAllRenderers = true;
        [Space(5)]
        [SerializeField] protected Outline[] outlines;
        
        protected OutlinesAnimationService _outlineService;

        public void Init(OutlinesAnimationService outlineService)
        {
            _outlineService = outlineService;
        }
        
        protected void Awake()
        {
            if (addOutlineToAllRenderers == true)
            {
                var renderesInChild = gameObject.GetComponentsInChildren<Renderer>();
                outlines = new Outline[renderesInChild.Length];
                
                for (int i = 0; i < renderesInChild.Length; i++)
                {
                    if (renderesInChild[i].TryGetComponent(out Outline outline) == false)
                    {
                        outline = renderesInChild[i].gameObject.AddComponent<Outline>();
                    }

                    outlines[i] = outline;
                }
            }
            
            SetActiveTotal(showOnAwake);
        }
        
        public abstract void Dispose();
        
        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);

            if (isActive == true)
            {
                EnableOutlines();
            }
            else
            {
                DisableOutlines();
            }
        }

        public virtual void SwitchOutlineIndex(int index)
        {
            Dispose();
            ChangeColor(index);
        }

        protected void EnableOutlines()
        {
            foreach (var outline in outlines)
            {
                outline.eraseRenderer = false;
            }
        }
        
        protected void DisableOutlines()
        {
            foreach (var outline in outlines)
            {
                outline.eraseRenderer = true;
            }
        }

        protected void ChangeColor(int colorIndex)
        {
            foreach (var outline in outlines)
            {
                outline.color = colorIndex;
            }
        }

        [Button("Activate")]
        public void ActivateDebug() => SetActiveTotal(true);
        
        [Button("Deactivate")]

        public void DeactivateDebug() => SetActiveTotal(false);
    }
}