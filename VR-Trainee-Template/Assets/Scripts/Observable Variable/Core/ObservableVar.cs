using System;
using System.Collections.Generic;

namespace ATG.Services.Quiz.Observable
{
    public class ObservableVar<T>: IObservableVar<T>
    {
        private readonly bool _ignoreEqualValue = false;

        private readonly List<Action<T>> _observers = new ();

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_ignoreEqualValue)
                {
                    if (_value?.Equals(value) ?? ReferenceEquals(_value, value)) return;
                }

                _value = value;

                NotifyObservers(_value);
            }
        }
        public T DefaultValue { get; }

        public ObservableVar() : this(default, ignoreEqualValue: true) { }
        
        public ObservableVar(T defaultValue, bool ignoreEqualValue = true)
        {
            DefaultValue = defaultValue;
            _value = DefaultValue;

            _ignoreEqualValue = ignoreEqualValue;
        }
        
        public ObserveDisposable Subscribe(Action<T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _observers.Add(callback);

            return new ObserveDisposable(() => _observers.Remove(callback));
        }

        private void NotifyObservers(T value)
        {
            foreach (var observer in _observers.ToArray())
            {
                observer?.Invoke(value);
            }
        }
        
        public void Dispose()
        {
            _observers.Clear();
        }
    }
}
