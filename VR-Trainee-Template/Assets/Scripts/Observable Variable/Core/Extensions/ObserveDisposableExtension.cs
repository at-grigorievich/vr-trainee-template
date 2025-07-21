using Unity.VisualScripting;

namespace ATG.Services.Quiz.Observable
{
    public static class ObserveDisposableExtension
    {
        public static ObserveDisposable AddTo(this ObserveDisposable disposable, CompositeObserveDisposable composite)
        {
            if (disposable != null && composite != null)
            {
                composite.Add(disposable);
            }

            return disposable;
        }
    }
}
