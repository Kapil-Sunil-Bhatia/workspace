using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public enum PlayerRole
    {
        Batsman,
        Bowler,
        AllRounder,
        WicketKeeper
    }

    public class Player
    {
        public int PlayerId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public PlayerRole Role { get; set; }
        
        [Required]
        public string Team { get; set; }
        
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
    }
}
