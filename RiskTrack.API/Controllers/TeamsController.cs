using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskTrack.Infrastructure.Data;
using RiskTrack.API.Domain.Entities;
using System.ComponentModel.Design;


namespace RiskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly RiskTrackDbContext _context;

        public TeamsController(RiskTrackDbContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _context.Teams.ToListAsync();
            return Ok(teams);
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound($"Team with ID {id} not found.");

            return Ok(team);
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamDto dto)
        {
            var team = new Team
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Leader = dto.Leader,
                ContactEmail = dto.ContactEmail
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Team created", TeamId = team.TeamId });
        }

        // PUT: api/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeamDto dto)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound($"Team with ID {id} not found.");

            team.CompanyId = dto.CompanyId;
            team.Name = dto.Name;
            team.Leader = dto.Leader;
            team.ContactEmail = dto.ContactEmail;

            _context.Teams.Update(team);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Team updated", TeamId = team.TeamId });
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound($"Team with ID {id} not found.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Team deleted", TeamId = id });
        }
    }
    public class TeamDto
    {
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Leader { get; set; }
        public string ContactEmail { get; set; } = default!;
    }
}

