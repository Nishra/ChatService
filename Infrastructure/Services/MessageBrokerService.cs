using Infrastructure.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Services
{
    public class MessageBrokerService
    {
        private readonly IOptions<MessageBrokerSettings> _messageBrokerSettings;
        public MessageBrokerService(IOptions<MessageBrokerSettings> messageBrokerSettings)
        {

            _messageBrokerSettings = messageBrokerSettings;

        }
        public IConnection CreateConnection()
        {
			try
			{
                var factory = new ConnectionFactory()
                {
                    HostName = _messageBrokerSettings.Value.Host,
                    UserName = _messageBrokerSettings.Value.Username,
                    Password = _messageBrokerSettings.Value.Password,
                    VirtualHost = _messageBrokerSettings.Value.VirtualHost
                };

                return factory.CreateConnection();
            }
			catch (Exception ex)
			{

				throw new Exception("Error occured when creating RabbitMQ connection", ex);
			}
        }
    }
}
