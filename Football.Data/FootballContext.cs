using Football.Domain;
using Microsoft.EntityFrameworkCore;

namespace Football.Data
{
    public class FootballContext : DbContext
    {
        public FootballContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<CompetitionTeams> CompetitionTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompetitionTeams>()
                .HasKey(cs => new { cs.CompetitionId, cs.TeamId });
        }
    }
}
