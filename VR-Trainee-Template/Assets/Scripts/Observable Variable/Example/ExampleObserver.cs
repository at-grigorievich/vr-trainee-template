using System;
using UnityEngine;

namespace ATG.Services.Quiz.Observable
{
    public class ExampleObserver: MonoBehaviour
    {
        public ExampleCreator creator;

        private IDisposable _disInt;
        private IDisposable _disString;

        private CompositeObserveDisposable _compositeDisposable;

        private void Start()
        {
            _compositeDisposable = new CompositeObserveDisposable();

            _disInt = creator._intValue.Subscribe(IntChanged).AddTo(_compositeDisposable);
            _disString = creator._stringValue.Subscribe(StringChanged).AddTo(_compositeDisposable);
        }

        private void IntChanged(int value)
        {
            Debug.Log("int: " + value);
        }

        private void StringChanged(string value)
        {
            Debug.Log("string: " + value);
        }

        [ContextMenu("Dispose int observing")]
        public void DisposeIntObserve()
        {
            _disInt.Dispose();
        }

        [ContextMenu("Dispose string observing")]
        public void DisposeStringObserve()
        {
            _disString.Dispose();
        }

        [ContextMenu("Composite disposable")]
        public void CompositeDisposable()
        {
            _compositeDisposable.Dispose();
        }
    }
}
