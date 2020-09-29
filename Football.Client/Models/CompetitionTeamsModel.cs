using System.Collections.Generic;

namespace Football.Client.Models
{
    public class CompetitionTeamsModel
    {
        public int Count { get; set; }

        public CompetitionModel Competition { get; set; }

        public List<TeamModel> Teams { get; set; }
    }
}
