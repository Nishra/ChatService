namespace Application.Interface
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}
