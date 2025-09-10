using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IContestService
    {
        Contest CreateContest(int matchId, string userId);
        Contest JoinContest(int contestId, string userId);
        UserTeam SubmitTeam(int contestId, string userId, List<int> matchPlayerIds);
        int GetTeamScore(int userTeamId);
        List<Contest> GetContestsByMatch(int matchId);
        Contest GetContestById(int contestId);
    }
}
