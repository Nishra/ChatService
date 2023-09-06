using Domain.Enum;

namespace Domain.Dictionary
{
    public static class DataDictionary
    {
      public static Dictionary<SeniorityLevel, double> SeniorityDictionary()
        {
            return new Dictionary<SeniorityLevel, double>
            {
                { SeniorityLevel.Junior, 0.4 },
                { SeniorityLevel.MidLevel, 0.6 },
                { SeniorityLevel.Senior, 0.8 },
                { SeniorityLevel.TeamLead, 0.5 },
            };
        }
    }
}
