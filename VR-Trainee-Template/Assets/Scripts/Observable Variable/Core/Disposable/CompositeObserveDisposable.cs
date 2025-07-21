using System;
using System.Collections.Generic;

namespace ATG.Services.Quiz.Observable
{
    public class CompositeObserveDisposable: IDisposable
    {
        private HashSet<IDisposable> _disposables;

        public void Add(IDisposable dis)
        {
            _disposables ??= new HashSet<IDisposable>();
            _disposables.Add(dis);
        }

        public void Remove(IDisposable dis)
        {
            if(_disposables == null) return;
            
            if (_disposables.Contains(dis))
            {
                _disposables.Remove(dis);
                dis.Dispose();
            }
        }

        public void Dispose()
        {
            if (_disposables == null) return;

            foreach (var d in _disposables)
            {
                d.Dispose();
            }

            _disposables = null;
        }
    }
}
