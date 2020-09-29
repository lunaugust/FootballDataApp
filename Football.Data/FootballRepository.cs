using Football.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Football.Data
{
    public class FootballRepository : IFootballRepository
    {
        private readonly FootballContext _context;

        public FootballRepository(FootballContext context)
        {
            _context = context;
        }

        public async Task<Competition> GetCompetitionAsync(Expression<Func<Competition, bool>> expression)
        {
            return await _context.Competitions.FirstOrDefaultAsync(expression);
        }

        public async Task<Competition> GetCompetitionWithChildrenAsync(Expression<Func<Competition, bool>> expression)
        {
            return await _context.Competitions.Include(x => x.CompetitionTeams).ThenInclude(x => x.Team).ThenInclude(x => x.Players).FirstOrDefaultAsync(expression);
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpsertTeamsAsync(List<Team> teams)
        {
            foreach (var team in teams)
            {
                if (await _context.Teams.AnyAsync(x => x.Id == team.Id))
                {
                    _context.Teams.Update(team);
                }
                else
                {
                    _context.Teams.Add(team);
                }

            }
        }

        public async Task UpsertCompetitionTeamsAsync(List<CompetitionTeams> competitionTeams)
        {
            foreach (var competitionTeam in competitionTeams)
            {
                if (!await _context.CompetitionTeams.AnyAsync(x => x.CompetitionId == competitionTeam.CompetitionId && x.TeamId == competitionTeam.TeamId))
                {
                    _context.CompetitionTeams.Add(competitionTeam);
                }
            }
        }
    }
}
