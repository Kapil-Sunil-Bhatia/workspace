using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnetapp.Services;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContestsController : ControllerBase
    {
        private readonly IContestService _contestService;
        
        public ContestsController(IContestService contestService)
        {
            _contestService = contestService;
        }
        
        [HttpGet("by-match/{matchId}")]
        public IActionResult GetContestsByMatch(int matchId)
        {
            var contests = _contestService.GetContestsByMatch(matchId);
            return Ok(contests);
        }
        
        [HttpPost]
        public IActionResult CreateContest([FromBody] CreateContestDto model)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var contest = _contestService.CreateContest(model.MatchId, userId);
            return Ok(contest);
        }
        
        [HttpPost("{contestId}/join")]
        public IActionResult JoinContest(int contestId)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var contest = _contestService.JoinContest(contestId, userId);
            return Ok(contest);
        }
        
        [HttpGet("{contestId}")]
        public IActionResult GetContest(int contestId)
        {
            var contest = _contestService.GetContestById(contestId);
            if (contest == null) return NotFound();
            return Ok(contest);
        }

        // [HttpGet]
        // public IActionResult GetAllContests()
        // {
        //     var contests = _contestService.GetAllContests();
        //     return Ok(contests);
        // }

        // [HttpPut("{id}")]
        // public IActionResult UpdateContest(int id, [FromBody] Contest model)
        // {
        //     var contest = _contestService.GetContestById(id);
        //     if (contest == null) return NotFound();
        //     contest.Name = model.Name;
        //     contest.CreatedByUserId = model.CreatedByUserId;
        //     contest.JoinedByUserId = model.JoinedByUserId;
        //     _contestService.UpdateContest(contest);
        //     return Ok(contest);
        // }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteContest(int id)
        // {
        //     var contest = _contestService.GetContestById(id);
        //     if (contest == null) return NotFound();
        //     _contestService.DeleteContest(contest);
        //     return Ok(new { message = "Contest deleted" });
        // }
    }
    
    public class CreateContestDto
    {
        public int MatchId { get; set; }
    }
}
