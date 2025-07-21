using UnityEngine;

namespace ATG.Services.Move
{
    public interface IMoveableService
    {
        float CurrentSpeed { get; }
        
        void SetActive(bool isActive);
        void MoveTo(Vector3 position);
        void PlaceTo(Vector3 position, Quaternion rotation);
        
        void LookAt(Vector3 lookAtPosition);
        
        bool CanReach(Vector3 inputPosition, out Vector3 resultPosition);
        void Stop();
    }
}