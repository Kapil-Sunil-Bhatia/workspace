using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly IContestService _contestService;
        
        public ScoreService(ApplicationDbContext context, IContestService contestService)
        {
            _context = context;
            _contestService = contestService;
        }
        
        public void UpdateScore(int matchPlayerId, int score)
        {
            var matchPlayer = _context.MatchPlayers.Find(matchPlayerId);
            if (matchPlayer == null) throw new ArgumentException("MatchPlayer not found");
            
            matchPlayer.Score = score;
            _context.SaveChanges();
        }
        
        public void DeclareResult(int contestId)
        {
            var contest = _context.Contests
                .Include(c => c.UserTeams)
                .FirstOrDefault(c => c.ContestId == contestId);
            
            if (contest == null) throw new ArgumentException("Contest not found");
            if (contest.UserTeams.Count != 2) throw new InvalidOperationException("Both users must submit teams");
            
            var team1 = contest.UserTeams.First();
            var team2 = contest.UserTeams.Last();
            
            var score1 = _contestService.GetTeamScore(team1.UserTeamId);
            var score2 = _contestService.GetTeamScore(team2.UserTeamId);
            
            contest.WinnerUserId = score1 > score2 ? team1.UserId : team2.UserId;
            contest.Status = ContestStatus.Completed;
            
            _context.SaveChanges();
        }
        
        public List<MatchPlayer> GetMatchPlayers(int matchId)
        {
            return _context.MatchPlayers
                .Include(mp => mp.Player)
                .Where(mp => mp.MatchId == matchId)
                .ToList();
        }
    }
}
