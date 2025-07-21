using UnityEngine;
using Random = UnityEngine.Random;

namespace ATG.Services.Move
{
    public interface INavigatablePoint
    {
        Vector3 GetRandomPointInRadiusXZ();
    }
    
    public class TargetNavigationPoint : MonoBehaviour, INavigatablePoint
    {
        [field: SerializeField] public float Radius { get; protected set; }
        
        public Vector3 Center => transform.position;
        
        public Vector3 GetRandomPointInRadiusXZ()
        {
            Vector2 randomOffset = Random.insideUnitCircle * Radius;
            return Center + new Vector3(randomOffset.x, 0, randomOffset.y);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
