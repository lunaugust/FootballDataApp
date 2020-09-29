using System.Collections.Generic;

namespace Football.Client.Models
{
    public class TeamModel
    {
        public int Id { get; set; }

        public string Tla { get; set; }

        public string ShortName { get; set; }

        public AreaModel Area { get; set; }

        public string Email { get; set; }

        public List<CompetitionModel> ActiveCompetitions { get; set; }

        public List<PlayerModel> Squad { get; set; }
    }
}
