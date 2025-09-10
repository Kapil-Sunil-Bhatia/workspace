using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        
        [Required]
        public string TeamA { get; set; }
        
        [Required]
        public string TeamB { get; set; }
        
        public DateTime Date { get; set; }
        
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
        public virtual ICollection<Contest> Contests { get; set; }
    }
}
