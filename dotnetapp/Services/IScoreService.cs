using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IScoreService
    {
        void UpdateScore(int matchPlayerId, int score);
        void DeclareResult(int contestId);
        List<MatchPlayer> GetMatchPlayers(int matchId);
    }
}
