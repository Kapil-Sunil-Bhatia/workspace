using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

namespace dotnetapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<MatchPlayer> MatchPlayers { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserTeamPlayer> UserTeamPlayers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // UserTeamPlayer composite key
            modelBuilder.Entity<UserTeamPlayer>()
                .HasKey(ut => new { ut.UserTeamId, ut.MatchPlayerId });
                
            // Contest relationships
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedContests)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.JoinedByUser)
                .WithMany(u => u.JoinedContests)
                .HasForeignKey(c => c.JoinedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<UserTeamPlayer>()
                .HasOne(utp => utp.UserTeam)
                .WithMany(ut => ut.UserTeamPlayers)
                .HasForeignKey(utp => utp.UserTeamId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade path

            modelBuilder.Entity<UserTeamPlayer>()
                .HasOne(utp => utp.MatchPlayer)
                .WithMany(mp => mp.UserTeamPlayers)
                .HasForeignKey(utp => utp.MatchPlayerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade path

            // Seed data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Players
            var players = new List<Player>();
            var playerNames = new[]
            {
                "Virat Kohli", "Rohit Sharma", "KL Rahul", "Hardik Pandya", "MS Dhoni",
                "Jasprit Bumrah", "Mohammad Shami", "Ravindra Jadeja", "Yuzvendra Chahal", "Shikhar Dhawan", "Rishabh Pant",
                "David Warner", "Steve Smith", "Pat Cummins", "Glenn Maxwell", "Marcus Stoinis",
                "Josh Hazlewood", "Adam Zampa", "Aaron Finch", "Matthew Wade", "Mitchell Starc"
            };
            
            var roles = new[] { PlayerRole.Batsman, PlayerRole.Bowler, PlayerRole.AllRounder, PlayerRole.WicketKeeper };
            var teams = new[] { "India", "Australia" };
            
            for (int i = 0; i < playerNames.Length; i++)
            {
                players.Add(new Player
                {
                    PlayerId = i + 1,
                    Name = playerNames[i],
                    Role = roles[i % 4],
                    Team = i < 11 ? "India" : "Australia"
                });
            }
            
            modelBuilder.Entity<Player>().HasData(players);
            
            // Seed Match
            modelBuilder.Entity<Match>().HasData(new Match
            {
                MatchId = 1,
                TeamA = "India",
                TeamB = "Australia",
                Date = DateTime.Now.AddDays(1)
            });
        }
    }
}
