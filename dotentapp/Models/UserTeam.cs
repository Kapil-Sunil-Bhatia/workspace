namespace dotnetapp.Models
{
    public class UserTeam
    {
        public int UserTeamId { get; set; }
        public int ContestId { get; set; }
        public string UserId { get; set; }
        
        public virtual Contest Contest { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<UserTeamPlayer> UserTeamPlayers { get; set; }
    }
}
