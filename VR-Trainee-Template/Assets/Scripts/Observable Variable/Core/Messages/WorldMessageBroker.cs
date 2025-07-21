using System;
using UnityEngine;

namespace ATG.Services.Quiz.Observable
{
    public sealed class WorldMessageBroker: IMessageBroker
    {
        private readonly MessageBroker _broker = new MessageBroker();

        public void Send<T>(T message) where T : IMessage
        {
            _broker.Send<T>(message);
        }

        public ObserveDisposable Subscribe<T>(Action<IMessage> receiver) where T : IMessage
        {
            return _broker.Subscribe<T>(receiver);
        }

        public void Dispose()
        {
            _broker.Dispose();
        }
    }
}
