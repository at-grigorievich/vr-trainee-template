using System;

namespace ATG.Services.Quiz.Observable
{
    public class ObserveDisposable: IDisposable
    {
        private readonly Action _disposeAction;
        private bool _isDisposed;

        public ObserveDisposable(Action disposeAction)
        {
            _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _disposeAction.Invoke();
            _isDisposed = true;
        }
    }
}
