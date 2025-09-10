using Microsoft.AspNetCore.Mvc;
using dotnetapp.Data;
using dotnetapp.Services;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoreService _scoreService;
        
        public MatchesController(ApplicationDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }
        
        [HttpGet]
        public IActionResult GetMatches()
        {
            var matches = _context.Matches.ToList();
            return Ok(matches);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetMatch(int id)
        {
            var match = _context.Matches
                .Include(m => m.MatchPlayers)
                .ThenInclude(mp => mp.Player)
                .FirstOrDefault(m => m.MatchId == id);
                
            if (match == null) return NotFound();
            
            return Ok(match);
        }
        
        [HttpGet("{id}/players")]
        public IActionResult GetMatchPlayers(int id)
        {
            var players = _scoreService.GetMatchPlayers(id);
            return Ok(players);
        }
    }
}
