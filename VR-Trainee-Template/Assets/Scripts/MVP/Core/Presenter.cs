using System;
using ATG.Activator;
using UnityEngine;
using VContainer.Unity;

namespace ATG.MVP
{
    public abstract class Presenter<T> : ActivateObject, IStartable, ITickable, IDisposable 
        where T : View
    {
        protected readonly T _view;
        
        public Transform Transform => _view.transform;

        public Presenter(T view)
        {
            _view = view;
        }

        public override void SetActiveVisual(bool isActive)
        {
            base.SetActiveVisual(isActive);
            _view.SetActiveVisual(isActive);
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            _view.SetActiveLogical(isActive);
        }
        
        public void SetScene(UnityEngine.SceneManagement.Scene nextScene) => _view.SetScene(nextScene);
        
        public void SetParent(Transform parent) => _view.SetParent(parent);
        
        public void SetLocalPosition(Vector3 localPosition) => _view.SetLocalPosition(localPosition);
        public void SetLocalRotation(Quaternion localRotation) => _view.SetLocalRotation(localRotation);
        
        public void SetLayer(int layerIndex) => _view.SetLayer(layerIndex);

        public virtual void Start() { }
        
        public virtual void Tick() { }
        
        public virtual void Dispose() { }
    }
}
