namespace Domain.Enum
{
    public class EnumMapping
    {
        private Dictionary<SeniorityLevel, Queues> senirorityLevelToQueuesMapping = new()
        {
            {SeniorityLevel.Junior, Queues.JuniorQueue },
            {SeniorityLevel.MidLevel, Queues.MidQueue },
            {SeniorityLevel.Senior, Queues.SeniorQueue },
            {SeniorityLevel.TeamLead, Queues.TeamLeadQueue }
        };

        public string GetQueueFromSenirorityLevel(SeniorityLevel seniorityLevel)
        {
            if (senirorityLevelToQueuesMapping.TryGetValue(seniorityLevel, out Queues queues))
            {
                return System.Enum.GetName(queues);
            }
            return System.Enum.GetName(Queues.SessionQueue); 
        }
    }
}
