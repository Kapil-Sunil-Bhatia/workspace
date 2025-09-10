using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnetapp.Services;
using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/user-teams")]
    [Authorize]
    public class UserTeamsController : ControllerBase
    {
        private readonly IContestService _contestService;
        private readonly ApplicationDbContext _context;
        
        public UserTeamsController(IContestService contestService, ApplicationDbContext context)
        {
            _contestService = contestService;
            _context = context;
        }
        
        [HttpPost("submit")]
        public IActionResult SubmitTeam([FromBody] SubmitTeamDto model)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var userTeam = _contestService.SubmitTeam(model.ContestId, userId, model.MatchPlayerIds);
            return Ok(userTeam);
        }
        
        [HttpGet("by-contest/{contestId}")]
        public IActionResult GetUserTeamByContest(int contestId)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var userTeam = _context.UserTeams
                .Include(ut => ut.UserTeamPlayers)
                .ThenInclude(utp => utp.MatchPlayer)
                .ThenInclude(mp => mp.Player)
                .FirstOrDefault(ut => ut.ContestId == contestId && ut.UserId == userId);
                
            if (userTeam == null) return NotFound();
            return Ok(userTeam);
        }
        
        [HttpGet("{userTeamId}/score")]
        public IActionResult GetTeamScore(int userTeamId)
        {
            var score = _contestService.GetTeamScore(userTeamId);
            return Ok(new { score });
        }

        [HttpGet]
        public IActionResult GetAllUserTeams()
        {
            var userTeams = _context.UserTeams.ToList();
            return Ok(userTeams);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserTeam(int id)
        {
            var userTeam = _context.UserTeams.Find(id);
            if (userTeam == null) return NotFound();
            return Ok(userTeam);
        }

        [HttpPost]
        public IActionResult CreateUserTeam([FromBody] UserTeam model)
        {
            _context.UserTeams.Add(model);
            _context.SaveChanges();
            return Ok(model);
        }

        // [HttpPut("{id}")]
        // public IActionResult UpdateUserTeam(int id, [FromBody] UserTeam model)
        // {
        //     var userTeam = _context.UserTeams.Find(id);
        //     if (userTeam == null) return NotFound();
        //     userTeam.Name = model.Name;
        //     userTeam.UserId = model.UserId;
        //     _context.SaveChanges();
        //     return Ok(userTeam);
        // }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteUserTeam(int id)
        // {
        //     var userTeam = _context.UserTeams.Find(id);
        //     if (userTeam == null) return NotFound();
        //     _context.UserTeams.Remove(userTeam);
        //     _context.SaveChanges();
        //     return Ok(new { message = "User team deleted" });
        // }
    }
    
    public class SubmitTeamDto
    {
        public int ContestId { get; set; }
        public List<int> MatchPlayerIds { get; set; }
    }
}
