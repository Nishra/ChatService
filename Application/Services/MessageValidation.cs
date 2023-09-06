using Application.Interface;
using Common.Utils;
using Domain.DTO;
using Domain.Enum;

namespace Application.Services
{
    public class MessageValidation : IMessageValidation
    {
        private readonly TeamCapacityCalculator _teamCapacityCalculator;
        public MessageValidation(TeamCapacityCalculator teamCapacityCalculator) 
        {
            _teamCapacityCalculator = teamCapacityCalculator;
        }
        public bool ValidateMessageQueue(Team currentTeam)
        {
            var totalTeamCapacity = _teamCapacityCalculator.CalculateTeamCapacity(currentTeam);
            Dictionary<SeniorityLevel, int> seniorityLevelQueueCapacity = _teamCapacityCalculator.SeniorityLevelQueueCapacity(currentTeam.Agents);
            var difference = totalTeamCapacity - seniorityLevelQueueCapacity.Values.Sum();

            var currentTeamQueues = currentTeam.Agents.Select(agent=> agent.SeniorityLevel).ToList();
            int currentSeniorityCapacity = 0;
            int currentTotalInSessionQueue = _teamCapacityCalculator.GetTotalMessageCountInQueue("SessionQueue");
            foreach (var seniorityLevelQueue in currentTeamQueues)
            {
                currentSeniorityCapacity += _teamCapacityCalculator.GetTotalMessageCountInQueue($"{seniorityLevelQueue}Queue");
            }

            if(currentSeniorityCapacity == totalTeamCapacity) return false;
            else if (totalTeamCapacity == currentTotalInSessionQueue + currentSeniorityCapacity) return false;
            else if (difference > 0) return true;
            else return false;
        }
    }
}
