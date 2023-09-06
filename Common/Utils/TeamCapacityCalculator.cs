using Domain.Dictionary;
using Domain.DTO;
using Domain.Enum;
using Infrastructure.Services;

namespace Common.Utils
{
    public class TeamCapacityCalculator
    {
        private readonly MessageBrokerService _messageBrokerService;
        
        public TeamCapacityCalculator(MessageBrokerService messageBrokerService)
        {
            _messageBrokerService = messageBrokerService;
            
        }
        public int CalculateTeamCapacity(Team team)
        {
            int totalCapacity = team.Agents.Sum(CalculateAgentCapacity);
            return totalCapacity;
        }

        public static int CalculateAgentCapacity(Agent agent)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime >= agent.ShiftStartTime.TimeOfDay &&
                currentTime <= agent.ShiftEndTime.TimeOfDay)
            {
                return agent.SeniorityLevel switch
                {
                    SeniorityLevel.Junior => (int)(agent.MaxConcurrentCapacity * 0.4),
                    SeniorityLevel.MidLevel => (int)(agent.MaxConcurrentCapacity * 0.6),
                    SeniorityLevel.Senior => (int)(agent.MaxConcurrentCapacity * 0.8),
                    SeniorityLevel.TeamLead => (int)(agent.MaxConcurrentCapacity * 0.5),
                    _ => throw new ArgumentException("Invalid Seniority Level"),
                };
            }
            else
            {
                //Agents are not there / shift is over / no capacity
                return 0;
            }
        }

        public int GetTotalMessageCountInQueue(string? sourceQueueName)
        {
            using var connection = _messageBrokerService.CreateConnection();
            using var channel = connection.CreateModel();
            // Declare the queue to ensure it exists.
            channel.QueueDeclare(queue: sourceQueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);

            // Get the message count in the queue.
            var messageCount = (int)channel.MessageCount(sourceQueueName);

            Console.WriteLine($"Total messages in {sourceQueueName}: {messageCount}");
            return messageCount;
        }

        public Dictionary<SeniorityLevel, int> SeniorityLevelQueueCapacity(List<Agent> agents)
        {
            int totalIncomingChats = GetTotalMessageCountInQueue(Enum.GetName(Queues.SessionQueue));

            // Define Seniority Multipliers
            Dictionary<SeniorityLevel, double> seniorityMultipliers = DataDictionary.SeniorityDictionary();

            // Calculate the capacity for each seniority level within the team.
            var seniorityCapacities = new Dictionary<SeniorityLevel, int>();

            foreach (var seniorityLevel in Enum.GetValues(typeof(SeniorityLevel)).Cast<SeniorityLevel>())
            {
                var agentsOfSeniority = agents.Where(agent => agent.SeniorityLevel == seniorityLevel);
                var capacity = (int)Math.Floor(agentsOfSeniority.Sum(agent => agent.MaxConcurrentCapacity) * seniorityMultipliers[seniorityLevel]);
                seniorityCapacities[seniorityLevel] = capacity;
            }

            var messagesToAssignBySeniority = new Dictionary<SeniorityLevel, int>();

            foreach (var seniorityLevel in Enum.GetValues(typeof(SeniorityLevel)).Cast<SeniorityLevel>())
            {
                var capacity = seniorityCapacities[seniorityLevel];
                var messagesToAssign = Math.Min(totalIncomingChats, capacity);
                messagesToAssignBySeniority[seniorityLevel] = messagesToAssign;
                totalIncomingChats -= messagesToAssign; // Update the remaining chats.
            }

            foreach (var messageCount in messagesToAssignBySeniority)
            {
                Console.WriteLine($"Messages to assign for {messageCount.Key}: {messageCount.Value}");
            }
            return messagesToAssignBySeniority;
        }
    }
}
