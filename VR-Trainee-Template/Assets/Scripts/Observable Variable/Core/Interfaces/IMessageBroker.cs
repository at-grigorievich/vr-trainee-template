using System;

namespace ATG.Services.Quiz.Observable
{
    public interface IMessageBroker
    {
        void Send<T>(T message) where T : IMessage;
        ObserveDisposable Subscribe<T>(Action<IMessage> receiver) where T : IMessage;

        void Dispose();
    }
}
