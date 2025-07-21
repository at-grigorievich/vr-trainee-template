using ATG.Services.Quiz.Observable;
using UnityEngine;
using UnityEngine.AI;

namespace ATG.Services.Move
{
    public class NavMeshMoveService: IMoveableService
    {
        private static int Priority = 40;
        
        private const float _smoothTime = 5f;
        
        private readonly IReadOnlyObservableVar<float> _speedVariable;
        private readonly NavMeshAgent _agent;
        
        private readonly NavMeshPath _path;
        private NavMeshHit _hit;
        
        public NavMeshMoveService(NavMeshAgent agent, IReadOnlyObservableVar<float> speedVariable)
        {
            _agent = agent;
            _agent.avoidancePriority = Priority++;
            
            _speedVariable = speedVariable;
            _path = new NavMeshPath();
        }

        public float CurrentSpeed => _agent.speed;

        public void SetActive(bool isActive)
        {
            _agent.enabled = isActive;
        }

        public void MoveTo(Vector3 position)
        {
            if(_agent.enabled == false) return;
            
            _agent.isStopped = false;
            _agent.speed = _speedVariable.Value;
            _agent.SetDestination(position);
            
            RotateToPosition(position);
        }

        public void PlaceTo(Vector3 position, Quaternion rotation)
        {
            SetActive(false);
            _agent.transform.position = position;
            _agent.transform.rotation = rotation;
            SetActive(true);
        }

        public void LookAt(Vector3 lookAtPosition)
        {
            lookAtPosition.y = _agent.transform.position.y;
            _agent.transform.LookAt(lookAtPosition);
        }

        public bool CanReach(Vector3 inputPosition, out Vector3 resultPosition)
        {
            resultPosition = inputPosition;
            
            if(_agent.enabled == false) return false;
            
            if (NavMesh.SamplePosition(inputPosition, out _hit, 0.01f, NavMesh.AllAreas) == false) 
                return false;
            
            if (_agent.CalculatePath(_hit.position, _path) == false) 
                return false;
            
            //Debug.DrawRay(resultPosition, Vector3.up * 10f, Color.red, 10f);
            
            resultPosition = _hit.position;
            return true;
        }

        public void Stop()
        {
            if(_agent.enabled == false) return;
            
            _agent.speed = 0f;
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        private void RotateToPosition(Vector3 position)
        {
            Transform transform = _agent.transform;
            
            Vector3 dir = position - transform.position;
            
            if(dir.sqrMagnitude == 0) return;
            
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir),_smoothTime * Time.deltaTime);
        }
    }
}