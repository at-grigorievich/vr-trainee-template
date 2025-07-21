using ATG.Services.Calibration;
using UnityEngine;

namespace ATG.MVP.VR
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class XrAgentView: View
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Collider mainCollider;
        
        private Rigidbody _rb;

        public Transform MainCameraTransform => mainCamera.transform;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }

        public override void SetActiveLogical(bool isActive)
        {
            base.SetActiveLogical(isActive);
            mainCamera.enabled = isActive;
        }

        public void Place(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void ResetOffset()
        {
            ResetPosition();
            ResetYawRotation();
        }

        private void ResetPosition()
        {
            Transform offset = MainCameraTransform.parent;
            
            Vector3 direction = transform.position - MainCameraTransform.position;
            direction.y = HeightCalibrationService.GetHeightFromPrefs();
            
            offset.localPosition = direction;
        }

        private void ResetYawRotation()
        {
            Transform offset = MainCameraTransform.parent;

            Vector3 eulerAngles = MainCameraTransform.localEulerAngles;
            eulerAngles.x = 0f;
            eulerAngles.y *= -1f;
            eulerAngles.z = 0f;
            
            offset.localEulerAngles = eulerAngles;
        }
    }
}