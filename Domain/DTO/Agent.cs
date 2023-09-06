using Domain.Enum;

namespace Domain.DTO
{
    public class Agent
    {
        public string AgentName { get; set; } = string.Empty;
        public SeniorityLevel SeniorityLevel { get; set; }
        public int MaxConcurrentCapacity { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
    }
}
