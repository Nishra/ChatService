using Domain.DTO;
using Domain.Enum;

namespace Common.SeedData
{
    public static class SeedAgentTeamData
    {

        /// <summary>
        /// Team A
        /// </summary>
        /// <returns></returns>
        #region Team A
        public static Agent GetAgent1Details()
        {
            return new()
            {
                AgentName = "Agent1",
                SeniorityLevel = SeniorityLevel.Junior,
                ShiftStartTime = DateTime.Parse("00:00:00"),
                ShiftEndTime = DateTime.Parse("20:35:00"),
                MaxConcurrentCapacity = 10
            };
        }

        public static Agent GetAgent2Details()
        {
            return new()
            {
                AgentName = "Agent2",
                SeniorityLevel = SeniorityLevel.Senior,
                ShiftStartTime = DateTime.Parse("00:00:00"),
                ShiftEndTime = DateTime.Parse("20:35:00"),
                MaxConcurrentCapacity = 10
            };
        }

        public static Team GetTeamADetails()
        {
            List<Agent> agents = new()
            {
                GetAgent1Details(),
                GetAgent2Details()
            };
            Team team = new()
            {
                TeamName = "TeamA",
                Agents = agents
            };

            return team;
        }
        #endregion


        /// <summary>
        /// Team B
        /// </summary>
        /// <returns></returns>
        /// 
        #region Team B
        public static Agent GetAgentB1Details()
        {
            return new()
            {
                AgentName = "AgentB1",
                SeniorityLevel = SeniorityLevel.Junior,
                ShiftStartTime = DateTime.Parse("11:00 PM"),
                ShiftEndTime = DateTime.Parse("12:00 AM"),
                MaxConcurrentCapacity = 10
            };
        }
        public static Agent GetAgentB2Details()
        {
            return new()
            {
                AgentName = "AgentB2",
                SeniorityLevel = SeniorityLevel.Senior,
                ShiftStartTime = DateTime.Parse("11:00 PM"),
                ShiftEndTime = DateTime.Parse("12:00 AM"),
                MaxConcurrentCapacity = 10
            };
        }

        public static Agent GetAgentB3Details()
        {
            return new()
            {
                AgentName = "AgentB3",
                SeniorityLevel = SeniorityLevel.Junior,
                ShiftStartTime = DateTime.Parse("11:00 PM"),
                ShiftEndTime = DateTime.Parse("12:00 AM"),
                MaxConcurrentCapacity = 10
            };
        }
        public static Team GetTeamBDetails()
        {
            List<Agent> agents = new()
            {
                GetAgentB1Details(),
                GetAgentB2Details(),
                GetAgentB3Details()
            };
            Team team = new()
            {
                TeamName = "TeamB",
                Agents = agents
            };

            return team;
        }
        #endregion
    }
}
