using ATG.Activator;
using UnityEngine;

namespace ATG.MVP
{
    public abstract class View: ActivateBehaviour
    {
        public void SetScene(UnityEngine.SceneManagement.Scene scene) => 
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, scene);

        public void SetParent(Transform parent) => transform.SetParent(parent);
        
        public void SetLocalPosition(Vector3 position) => transform.localPosition = position;
        public void SetLocalRotation(Quaternion rotation) => transform.localRotation = rotation;
        
        public void SetLayer(int layerIndex) => gameObject.layer = layerIndex;
    }
}