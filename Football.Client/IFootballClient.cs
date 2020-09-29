using Football.Client.Models;
using System.Threading.Tasks;

namespace Football.Client
{
    public interface IFootballClient
    {
        Task<CompetitionModel> GetCompetitionByCodeAsync(string code);

        Task<CompetitionTeamsModel> GetTeamsByCompetitionAsync(int competitionId);

        Task<TeamModel> GetTeamWithPlayersAsync(int teamId);

    }
}
