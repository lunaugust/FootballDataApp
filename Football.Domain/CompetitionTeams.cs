namespace Football.Domain
{
    public class CompetitionTeams
    {
        public int CompetitionId { get; set; }
        public int TeamId { get; set; }
        public Competition Competition { get; set; }
        public Team Team { get; set; }
    }
}
