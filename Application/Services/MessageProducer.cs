using Application.Interface;
using Domain.Enum;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly ILogger<MessageProducer> _logger;
        private readonly MessageBrokerService _messageBrokerService;
        public MessageProducer(ILogger<MessageProducer> logger, MessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _messageBrokerService = messageBrokerService;
        }
        public void SendMessage<T>(T message)
        {
            try
            {
                _logger.LogInformation($"{nameof(MessageProducer)} : {nameof(SendMessage)} Started");

                var connection = _messageBrokerService.CreateConnection();

                using var channel = connection.CreateModel();

                channel.QueueDeclare(Enum.GetName(Queues.SessionQueue), durable: true, autoDelete: false, exclusive: false);

                var jsonString = JsonSerializer.Serialize(message);

                var body = Encoding.UTF8.GetBytes(jsonString);

                channel.BasicPublish("", Enum.GetName(Queues.SessionQueue), body: body);

                _logger.LogInformation($"{nameof(MessageProducer)} : {nameof(SendMessage)} Ended");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(MessageProducer)} : {nameof(SendMessage)}", ex.Message);
                throw;
            }
        }
    }
}
