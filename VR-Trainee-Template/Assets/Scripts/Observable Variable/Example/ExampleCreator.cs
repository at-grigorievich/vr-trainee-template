using System.Collections;
using UnityEngine;

namespace ATG.Services.Quiz.Observable
{
    public class ExampleCreator: MonoBehaviour
    {
        public IObservableVar<int> _intValue;
        public IObservableVar<string> _stringValue;

        private void Awake()
        {
            _intValue = new ObservableVar<int>(5, true);
            _stringValue = new ObservableVar<string>("asf", true);
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                _intValue.Value = UnityEngine.Random.Range(1, 25);
                _stringValue.Value = "test" + UnityEngine.Random.Range(2, 100);
            }
        }
    }
}
