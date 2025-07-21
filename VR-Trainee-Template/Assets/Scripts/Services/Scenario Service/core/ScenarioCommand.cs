using System;
using ATG.Command;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ATG.Services.Scenario
{
    public abstract class ScenarioCommand: MonoBehaviour, ICommand
    {
        [SerializeField] private UnityEvent onExecuteCallbacks;
        [SerializeField] private UnityEvent onCompleteCallbacks;
        
        public event Action<bool> OnCompleted;
        
        public virtual void Execute()
        {
            onExecuteCallbacks?.Invoke();
        }

        public virtual void Resolve(IObjectResolver resolver){}
        public virtual void Dispose() {}
        
        protected virtual void OnComplete(bool result)
        {
            onCompleteCallbacks?.Invoke();
            
            OnCompleted?.Invoke(result);
        }
    }
}