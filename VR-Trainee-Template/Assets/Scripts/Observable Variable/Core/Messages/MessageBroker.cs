using System;
using System.Collections.Generic;

namespace ATG.Services.Quiz.Observable
{
    public sealed class MessageBroker: IMessageBroker, IDisposable
    {
        private readonly Dictionary<Type, ObservableVar<IMessage>> _dic = new Dictionary<Type, ObservableVar<IMessage>>();
        
        public void Send<T>(T message) where T: IMessage
        {
            Type type = message.GetType();

            if (_dic.TryGetValue(type, out var observe1))
            {
                observe1.Value = message;
            }
            else
            {
                var observe = new ObservableVar<IMessage>();
                _dic.Add(type, observe);
                observe.Value = message;
            }
        }

        public ObserveDisposable Subscribe<T>(Action<IMessage> receiver ) where T : IMessage
        {
            Type type = typeof(T);

            if (_dic.TryGetValue(type, out var observe1))
            {
                return observe1.Subscribe(receiver);
            }
            
            var observe = new ObservableVar<IMessage>();
            _dic.Add(type, observe);

            return observe.Subscribe(receiver);
        }
        
        public void Dispose()
        {
            foreach(var observableVar in _dic.Values)
            {
                observableVar.Dispose();
            }

            _dic.Clear();
        }
    }
}
