using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Services
{
    public class ContestService : IContestService
    {
        private readonly ApplicationDbContext _context;
        
        public ContestService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Contest CreateContest(int matchId, string userId)
        {
            var contest = new Contest
            {
                MatchId = matchId,
                CreatedByUserId = userId,
                Status = ContestStatus.Pending
            };
            
            _context.Contests.Add(contest);
            _context.SaveChanges();
            
            return contest;
        }
        
        public Contest JoinContest(int contestId, string userId)
        {
            var contest = _context.Contests.Find(contestId);
            if (contest == null) throw new ArgumentException("Contest not found");
            if (contest.JoinedByUserId != null) throw new InvalidOperationException("Contest is full");
            if (contest.CreatedByUserId == userId) throw new InvalidOperationException("Cannot join own contest");
            
            contest.JoinedByUserId = userId;
            contest.Status = ContestStatus.Ongoing;
            _context.SaveChanges();
            
            return contest;
        }
        
        public UserTeam SubmitTeam(int contestId, string userId, List<int> matchPlayerIds)
        {
            if (matchPlayerIds.Count != 11) throw new ArgumentException("Team must have exactly 11 players");
            
            // Check if user already has a team for this contest
            var existingTeam = _context.UserTeams
                .FirstOrDefault(ut => ut.ContestId == contestId && ut.UserId == userId);
            
            if (existingTeam != null) throw new InvalidOperationException("Team already submitted");
            
            // Validate all players belong to the contest's match
            var contest = _context.Contests.Find(contestId);
            var validPlayerIds = _context.MatchPlayers
                .Where(mp => mp.MatchId == contest.MatchId)
                .Select(mp => mp.MatchPlayerId)
                .ToList();
            
            if (!matchPlayerIds.All(id => validPlayerIds.Contains(id)))
                throw new ArgumentException("Invalid players selected");
            
            var userTeam = new UserTeam
            {
                ContestId = contestId,
                UserId = userId
            };
            
            _context.UserTeams.Add(userTeam);
            _context.SaveChanges();
            
            var userTeamPlayers = matchPlayerIds.Select(mpId => new UserTeamPlayer
            {
                UserTeamId = userTeam.UserTeamId,
                MatchPlayerId = mpId
            }).ToList();
            
            _context.UserTeamPlayers.AddRange(userTeamPlayers);
            _context.SaveChanges();
            
            return userTeam;
        }
        
        public int GetTeamScore(int userTeamId)
        {
            return _context.UserTeamPlayers
                .Where(utp => utp.UserTeamId == userTeamId)
                .Include(utp => utp.MatchPlayer)
                .Sum(utp => utp.MatchPlayer.Score);
        }
        
        public List<Contest> GetContestsByMatch(int matchId)
        {
            return _context.Contests
                .Where(c => c.MatchId == matchId)
                .Include(c => c.CreatedByUser)
                .Include(c => c.JoinedByUser)
                .Include(c => c.Match)
                .ToList();
        }
        
        public Contest GetContestById(int contestId)
        {
            return _context.Contests
                .Include(c => c.Match)
                .Include(c => c.CreatedByUser)
                .Include(c => c.JoinedByUser)
                .Include(c => c.UserTeams)
                .FirstOrDefault(c => c.ContestId == contestId);
        }
    }
}
