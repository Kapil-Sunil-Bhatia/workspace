using Microsoft.AspNetCore.Identity;

namespace dotnetapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; } = false;
        public virtual ICollection<Contest> CreatedContests { get; set; }
        public virtual ICollection<Contest> JoinedContests { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}
