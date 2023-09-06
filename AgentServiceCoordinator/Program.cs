using Common.SeedData;
using Common.Utils;
using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
class Program
{
    static void Main()
    {
        Team team = SeedAgentTeamData.GetTeamADetails();
        //Team team = SeedAgentTeamData.GetTeamBDetails();

        IConfiguration Configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        var serviceProvider = new ServiceCollection()
                    .Configure<MessageBrokerSettings>(Configuration.GetSection("MessageBrokerSettings").Bind)
                    .BuildServiceProvider();

        IOptions<MessageBrokerSettings> messageBrokerSettings = serviceProvider.GetRequiredService<IOptions<MessageBrokerSettings>>();

        MessageBrokerService messageBrokerService = new(messageBrokerSettings); 
        TeamCapacityCalculator teamCapacityCalculator = new(messageBrokerService); 
        DistributeMessages distributeMessages = new(messageBrokerService, teamCapacityCalculator);

        int agentCapacity = teamCapacityCalculator.CalculateTeamCapacity(team);

        Console.WriteLine($"{team.TeamName} Capacity: {agentCapacity} \n");
        Console.WriteLine($"{team.TeamName} Max Queue Length: {agentCapacity * 1.5} \n");
        distributeMessages.PublishMessageToAgentQueue(team.Agents);
        Console.WriteLine("Coordinated Chats Successfully \n");
        Console.ReadLine();
    }
}
