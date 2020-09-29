using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football.Domain
{
    public class Team
    {
        public Team()
        {
            CompetitionTeams = new List<CompetitionTeams>();
            Players = new List<Player>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int Id { get; set; }

        public string Tla { get; set; }

        public string ShortName { get; set; }

        public string AreaName { get; set; }

        public string Email { get; set; }

        public List<CompetitionTeams> CompetitionTeams { get; set; }

        public List<Player> Players { get; set; }
    }
}
