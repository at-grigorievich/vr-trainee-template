using System;

namespace ATG.Services.Quiz.Observable
{
    public sealed class LocalMessageBroker: IMessageBroker
    {
        private readonly MessageBroker _broker = new MessageBroker();

        public void Dispose()
        {
            _broker.Dispose();
        }
        
        public void Send<T>(T message) where T : IMessage
        {
            _broker.Send<T>(message);
        }

        public ObserveDisposable Subscribe<T>(Action<IMessage> receiver) where T : IMessage
        {
            return _broker.Subscribe<T>(receiver);
        }
    }
}
