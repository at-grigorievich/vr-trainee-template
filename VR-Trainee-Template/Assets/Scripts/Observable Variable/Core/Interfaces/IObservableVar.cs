using System;

namespace ATG.Services.Quiz.Observable
{
    public interface IReadOnlyObservableVar<out T>: IDisposable
    {
        T Value { get; }
        T DefaultValue { get; }
        
        ObserveDisposable Subscribe(Action<T> callback);
    }
    
    public interface IObservableVar<T>: IReadOnlyObservableVar<T>
    {
        new T Value { get; set; }
    }
}
