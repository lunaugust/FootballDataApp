using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football.Domain
{
    public class Competition
    {
        public Competition()
        {
            CompetitionTeams = new List<CompetitionTeams>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string AreaName { get; set; }

        public List<CompetitionTeams> CompetitionTeams { get; set; }
    }
}
