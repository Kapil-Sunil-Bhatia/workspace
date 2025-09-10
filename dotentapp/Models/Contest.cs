namespace dotnetapp.Models
{
    public enum ContestStatus
    {
        Pending,
        Ongoing,
        Completed
    }

    public class Contest
    {
        public int ContestId { get; set; }
        public int MatchId { get; set; }
        public string CreatedByUserId { get; set; }
        public string? JoinedByUserId { get; set; }
        public ContestStatus Status { get; set; } = ContestStatus.Pending;
        public string? WinnerUserId { get; set; }
        
        public virtual Match Match { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ApplicationUser? JoinedByUser { get; set; }
        public virtual ApplicationUser? WinnerUser { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}
