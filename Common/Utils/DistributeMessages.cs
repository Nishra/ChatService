using Domain.DTO;
using Domain.Enum;
using Infrastructure.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Common.Utils
{
    public class DistributeMessages
    {
        private readonly MessageBrokerService _messageBrokerService;
        private readonly TeamCapacityCalculator _teamCapacityCalculator;
        public DistributeMessages(MessageBrokerService messageBrokerService,
                                  TeamCapacityCalculator teamCapacityCalculator)
        {
            _messageBrokerService = messageBrokerService;
            _teamCapacityCalculator = teamCapacityCalculator;
        }
        public void PublishMessageToAgentQueue(List<Agent> agents)
        {
            using IConnection connection = _messageBrokerService.CreateConnection();
            using IModel publishChannel = connection.CreateModel();

            string? sourceQueueName = Enum.GetName(Queues.SessionQueue);

            publishChannel.QueueDeclare(queue: sourceQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            Dictionary<SeniorityLevel, int> seniorityCapacities = _teamCapacityCalculator.SeniorityLevelQueueCapacity(agents);

            foreach (var seniorityCapacity in seniorityCapacities.Where(x=> x.Value !=0)) 
            {
                CreateConsumer(connection, $"{seniorityCapacity.Key}Queue", sourceQueueName, seniorityCapacity.Value);
            }
        }

        private static void CreateConsumer(IConnection connection, string seniorityQueueName, string? sourceQueueName, int consumerNumber)
        {

            using var consumeChannel = connection.CreateModel();

            consumeChannel.QueueDeclare(queue: seniorityQueueName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            ConsumeSourceQueue(sourceQueueName,consumeChannel,consumerNumber, seniorityQueueName);
        }

        private static void ConsumeSourceQueue(string? sourceQueueName, IModel consumeChannel, int consumerNumber, string seniorityQueueName)
        {

            for (int i = 0; i < consumerNumber; i++)
            {

                var result = consumeChannel.BasicGet(queue: sourceQueueName, autoAck: false);

                if (result != null)
                {
                    var body = result.Body.ToArray();

                    var jsonString = JsonSerializer.Serialize(body);

                    var message = Encoding.UTF8.GetBytes(jsonString);

                    consumeChannel.BasicPublish("", seniorityQueueName, body: message);

                    //var message2 = Encoding.UTF8.GetString(body);
                    //Console.WriteLine($"Received message2: {message2}");

                    // Acknowledge the message to remove it from the queue
                    consumeChannel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);
                }
                else
                {
                    Console.WriteLine("No messages found in the queue.");
                } 
            }
        }

    }
}
