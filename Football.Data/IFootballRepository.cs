using Football.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Football.Data
{
    public interface IFootballRepository
    {
        Task<Competition> GetCompetitionAsync(Expression<Func<Competition, bool>> expression);
        Task<Competition> GetCompetitionWithChildrenAsync(Expression<Func<Competition, bool>> expression);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task UpsertTeamsAsync(List<Team> teams);
        Task UpsertCompetitionTeamsAsync(List<CompetitionTeams> competitionTeams);
        Task<bool> SaveChangesAsync();
    }
}
