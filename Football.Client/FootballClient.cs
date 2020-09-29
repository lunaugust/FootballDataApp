using Flurl;
using Flurl.Http;
using Football.Client.Models;
using System.Threading.Tasks;

namespace Football.Client
{
    public class FootballClient : IFootballClient
    {
        private readonly string _url;
        private readonly string _headerKey;
        private readonly string _token;
        private readonly int _timeout;

        public FootballClient(string Url, string HeaderKey, string Token, int Timeout)
        {
            _url = Url;
            _headerKey = HeaderKey;
            _token = Token;
            _timeout = Timeout;
        }


        public async Task<CompetitionModel> GetCompetitionByCodeAsync(string code)
        {
            return await _url.AppendPathSegments("competitions", code)
                .AllowHttpStatus(System.Net.HttpStatusCode.NotFound)
                .WithHeader(_headerKey, _token)
                .WithTimeout(_timeout)
                .GetJsonAsync<CompetitionModel>();
        }

        public async Task<CompetitionTeamsModel> GetTeamsByCompetitionAsync(int competitionId)
        {
            return await _url.AppendPathSegments("competitions", competitionId, "teams")
                .WithHeader(_headerKey, _token)
                .WithTimeout(_timeout)
                .GetJsonAsync<CompetitionTeamsModel>();
        }

        public async Task<TeamModel> GetTeamWithPlayersAsync(int teamId)
        {
            return await _url.AppendPathSegments("teams", teamId)
                .WithHeader(_headerKey, _token)
                .WithTimeout(_timeout)
                .GetJsonAsync<TeamModel>();
        }
    }
}
