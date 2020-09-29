using Football.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Football.API.Controllers
{
    [Route("total-players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IFootballRepository _footballRepository;

        public PlayerController(IFootballRepository footballRepository)
        {
            _footballRepository = footballRepository;
        }

        [HttpGet("{leagueCode}")]
        public async Task<IActionResult> Get(string leagueCode)
        {
            var competition = await _footballRepository.GetCompetitionWithChildrenAsync(x => x.Code.Equals(leagueCode));
            if (competition == null)
            {
                return NotFound(new { message = "Not found" });
            }

            var total = 0;

            competition.CompetitionTeams.ForEach(x => total += x.Team.Players.Count);

            return Ok(new { total });
        }
    }
}
