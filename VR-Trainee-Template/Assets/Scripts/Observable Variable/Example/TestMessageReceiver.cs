using UnityEngine;

namespace ATG.Services.Quiz.Observable
{
    public class TestMessageReceiver
    {
        private readonly IMessageBroker _broker;

        public TestMessageReceiver(IMessageBroker broker)
        {
            _broker = broker;
        }

        private ObserveDisposable _dis;

        [ContextMenu("Subscribe to message")]
        public void Subscribe()
        {
            if(_dis != null)
            {
                _dis.Dispose();
            }
            _dis = _broker.Subscribe<TestMessage>(OnMessageReceived);
        }

        private void OnMessageReceived(IMessage mes)
        {
            Debug.Log("received");
        }
    }
}
