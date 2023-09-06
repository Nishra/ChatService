namespace Domain.DTO
{
    public class Team
    {
        public string TeamName { get; set; } = string.Empty;
        public required List<Agent> Agents { get; set; }
    }
}
