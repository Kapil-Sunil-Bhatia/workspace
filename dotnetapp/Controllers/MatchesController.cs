using Microsoft.AspNetCore.Mvc;
using dotnetapp.Data;
using dotnetapp.Services;
using dotnetapp.Models;
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
        public IActionResult GetAllMatches()
        {
            var matches = _context.Matches.ToList();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public IActionResult GetMatch(int id)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return NotFound();
            return Ok(match);
        }

        [HttpPost]
        public IActionResult CreateMatch([FromBody] Match model)
        {
            _context.Matches.Add(model);
            _context.SaveChanges();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMatch(int id, [FromBody] Match model)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return NotFound();
            match.TeamA = model.TeamA;
            match.TeamB = model.TeamB;
            match.Date = model.Date;
            _context.SaveChanges();
            return Ok(match);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMatch(int id)
        {
            var match = _context.Matches.Find(id);
            if (match == null) return NotFound();
            _context.Matches.Remove(match);
            _context.SaveChanges();
            return Ok(new { message = "Match deleted" });
        }
        
        [HttpGet("{id}/players")]
        public IActionResult GetMatchPlayers(int id)
        {
            var players = _scoreService.GetMatchPlayers(id);
            return Ok(players);
        }
    }
}
