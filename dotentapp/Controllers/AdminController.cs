using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnetapp.Data;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoreService _scoreService;
        
        public AdminController(ApplicationDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }
        
        [HttpPost("matches")]
        public IActionResult CreateMatch([FromBody] CreateMatchDto model)
        {
            var match = new Match
            {
                TeamA = model.TeamA,
                TeamB = model.TeamB,
                Date = model.Date
            };
            
            _context.Matches.Add(match);
            _context.SaveChanges();
            
            return Ok(match);
        }
        
        [HttpPost("matches/{matchId}/players")]
        public IActionResult AddPlayerToMatch(int matchId, [FromBody] AddPlayerDto model)
        {
            var player = _context.Players.Find(model.PlayerId);
            if (player == null) return BadRequest("Player not found");
            
            var matchPlayer = new MatchPlayer
            {
                MatchId = matchId,
                PlayerId = model.PlayerId,
                Score = 0
            };
            
            _context.MatchPlayers.Add(matchPlayer);
            _context.SaveChanges();
            
            return Ok(matchPlayer);
        }
        
        [HttpPost("matches/{matchId}/players/bulk")]
        public IActionResult BulkAddPlayers(int matchId, [FromBody] BulkAddPlayersDto model)
        {
            var matchPlayers = model.PlayerIds.Select(playerId => new MatchPlayer
            {
                MatchId = matchId,
                PlayerId = playerId,
                Score = 0
            }).ToList();
            
            _context.MatchPlayers.AddRange(matchPlayers);
            _context.SaveChanges();
            
            return Ok(matchPlayers);
        }
        
        [HttpPost("scores")]
        public IActionResult UpdateScore([FromBody] UpdateScoreDto model)
        {
            _scoreService.UpdateScore(model.MatchPlayerId, model.Score);
            return Ok(new { message = "Score updated" });
        }
        
        [HttpPost("results/declare")]
        public IActionResult DeclareResult([FromBody] DeclareResultDto model)
        {
            _scoreService.DeclareResult(model.ContestId);
            return Ok(new { message = "Result declared" });
        }
    }
    
    public class CreateMatchDto
    {
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public DateTime Date { get; set; }
    }
    
    public class AddPlayerDto
    {
        public int PlayerId { get; set; }
    }
    
    public class BulkAddPlayersDto
    {
        public List<int> PlayerIds { get; set; }
    }
    
    public class UpdateScoreDto
    {
        public int MatchPlayerId { get; set; }
        public int Score { get; set; }
    }
    
    public class DeclareResultDto
    {
        public int ContestId { get; set; }
    }
}
