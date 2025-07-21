using UnityEngine;

namespace ATG.Services.Move
{
    public class TargetNavigationPointSet: MonoBehaviour, INavigatablePoint
    {
        private INavigatablePoint[] _set;

        private void Awake()
        {
            _set = GetComponentsInChildren<INavigatablePoint>();
        }

        public Vector3 GetRandomPointInRadiusXZ()
        {
            return GetRndPoint().GetRandomPointInRadiusXZ();
        }

        private INavigatablePoint GetRndPoint()
        {
            int rndIndex = UnityEngine.Random.Range(0, _set.Length);
            return _set[rndIndex];
        }
    }
}