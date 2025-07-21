using System;

namespace ATG.Command
{
    public interface ICommand: IDisposable
    {
        event Action<bool> OnCompleted;
        
        void Execute();
    }
}