using System;
using ATG.Activator;
using UnityEngine;

namespace ATG.Services.EventObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionObject: ActivateBehaviour
    {
        [SerializeField] private bool activateOnAwake = true;

        private Rigidbody _rb;
        private Collider[] _colliders;
        
        public event Action<Collision> OnCollisionEntered;
        public event Action<Collision> OnCollisionExited;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();

            _rb.isKinematic = false;
            foreach (var col in _colliders)
            {
                col.isTrigger = false;
            }
            
            SetActiveTotal(activateOnAwake);
        }
        
        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            
            foreach (var col in _colliders)
            {
                col.enabled = isActive;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEntered?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExited?.Invoke(other);
        }
    }
}