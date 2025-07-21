using System;
using ATG.Activator;
using UnityEngine;

namespace ATG.Services.EventObject
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObject : ActivateBehaviour
    {
        [SerializeField] private bool activateOnAwake = true;

        private Collider _collider;

        public event Action<GameObject> OnTriggerEntered;
        public event Action<GameObject> OnTriggerExited;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            
            SetActiveTotal(activateOnAwake);
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            _collider.enabled = isActive;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExited?.Invoke(other.gameObject);
        }
    }
}
