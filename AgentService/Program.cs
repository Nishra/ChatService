using Common.SeedData;
using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main()
    {
        //Current Agent
        Agent agent = SeedAgentTeamData.GetAgent1Details();
        //Agent agent = SeedAgentTeamData.GetAgent2Details();

        Console.WriteLine($"Hello {agent.AgentName} - {agent.SeniorityLevel} \n Welcome to Calamatta Cuschieri Finance Service! \n");

        IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .Configure<MessageBrokerSettings>(Configuration.GetSection("MessageBrokerSettings").Bind)
            .BuildServiceProvider();

        IOptions<MessageBrokerSettings> messageBrokerSettings = serviceProvider.GetRequiredService<IOptions<MessageBrokerSettings>>();

        MessageBrokerService messageBrokerService = new(messageBrokerSettings);
        IConnection connection = messageBrokerService.CreateConnection();
        using IModel channel = connection.CreateModel();

        channel.QueueDeclare($"{agent.SeniorityLevel}Queue", durable: true, exclusive: false, autoDelete: false);

        EventingBasicConsumer consumer = new(channel);

        #region Consume Queue
        consumer.Received += (sender, args) =>
            {
                // getting the byte[]
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"A new message recieved : \n");
                Console.WriteLine($"{message}\n");
                Console.WriteLine($"Acknowledge message ? \n");

                // Prompt the user for input and validate it
                Console.Write("Enter 'OK' or 'NO': ");
                string? shouldAcknowledge = Console.ReadLine();

                if (!string.IsNullOrEmpty(shouldAcknowledge) &&
                    !string.IsNullOrWhiteSpace(shouldAcknowledge) &&
                    shouldAcknowledge.Equals("OK"))
                {
                    // Acknowledge the message.
                    channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                    Console.WriteLine($"Acknowledge message successfully.");
                    Console.ReadKey();
                }
                else
                {
                    channel.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: true);
                    Console.WriteLine($"Requeue message");
                    Console.ReadKey();
                }
            };
        #endregion

        channel.BasicConsume($"{agent.SeniorityLevel}Queue", false, consumer);

        Console.ReadKey();

    }
}