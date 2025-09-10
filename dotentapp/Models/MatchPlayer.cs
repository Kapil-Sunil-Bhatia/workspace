namespace dotnetapp.Models
{
    public class MatchPlayer
    {
        public int MatchPlayerId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Score { get; set; } = 0;
        
        public virtual Match Match { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<UserTeamPlayer> UserTeamPlayers { get; set; }
    }
}
