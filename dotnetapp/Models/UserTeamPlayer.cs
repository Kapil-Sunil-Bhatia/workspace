namespace dotnetapp.Models
{
    public class UserTeamPlayer
    {
        public int UserTeamId { get; set; }
        public int MatchPlayerId { get; set; }
        
        public virtual UserTeam UserTeam { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
