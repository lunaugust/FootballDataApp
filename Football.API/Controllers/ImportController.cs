using AutoMapper;
using Football.Client;
using Football.Client.Models;
using Football.Data;
using Football.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football.API.Controllers
{
    [Route("import-league")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFootballRepository _footballRepository;
        private readonly IFootballClient _footballClient;

        public ImportController(IMapper mapper, IFootballRepository footballRepository, IFootballClient footballClient)
        {
            _mapper = mapper;
            _footballRepository = footballRepository;
            _footballClient = footballClient;
        }

        /// <summary>
        /// Import from football-data API one Competition, his Teams and Players.
        /// </summary>
        /// <param name="leagueCode"></param>
        /// <returns>Status code and message</returns>
        /// <response code="201">If import success</response>
        /// <response code="409">If league is already imported</response> 
        /// <response code="404">If league does not exist on external API</response>
        /// <response code="504">If exception occurred</response> 
        [HttpGet("{leagueCode}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string leagueCode)
        {
            var competition = await _footballRepository.GetCompetitionAsync(x => x.Code.Equals(leagueCode));
            if (competition != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { message = "League already imported" });
            }

            var competitionModel = await _footballClient.GetCompetitionByCodeAsync(leagueCode);
            if (competitionModel.Id == 0)
            {
                return NotFound(new { message = "Not found" });
            }

            competition = _mapper.Map<Competition>(competitionModel);

            var competitionTeamModel = await _footballClient.GetTeamsByCompetitionAsync(competitionModel.Id);

            var teams = _mapper.Map<List<Team>>(competitionTeamModel.Teams);

            await GetPlayersForTeams(teams);

            _footballRepository.Add(competition);

            await _footballRepository.UpsertTeamsAsync(teams);

            await _footballRepository.UpsertCompetitionTeamsAsync(GetCompetitionTeams(competition, teams));

            await _footballRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), leagueCode, new { message = "Successfully imported" });

        }

        private static List<CompetitionTeams> GetCompetitionTeams(Competition competition, List<Team> teams)
        {
            List<CompetitionTeams> competitionTeams = new List<CompetitionTeams>();
            competitionTeams.AddRange(teams.Select(team => new CompetitionTeams() { CompetitionId = competition.Id, Competition = competition, TeamId = team.Id, Team = team }));
            return competitionTeams;
        }

        private async Task GetPlayersForTeams(List<Team> teams)
        {
            List<Task<TeamModel>> tasks = new List<Task<TeamModel>>();
            for (int i = 0; i < teams.Count - 1; i++)
            {
                tasks.Add(_footballClient.GetTeamWithPlayersAsync(teams[i].Id));
                if (i == 5) // I am limiting the amount of players to get from the API because of the restriction of 10 request per minute.
                {
                    break;
                }
            }

            var results = await Task.WhenAll(tasks);

            foreach (var teamModel in results)
            {
                _mapper.Map(teamModel, teams.FirstOrDefault(x => x.Id.Equals(teamModel.Id)));
            }
        }
    }
}
