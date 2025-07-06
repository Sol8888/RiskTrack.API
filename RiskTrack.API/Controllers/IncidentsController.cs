using System.ComponentModel.DataAnnotations;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskTrack.API.Domain.Entities;
using RiskTrack.Infrastructure.Data;

namespace RiskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly RiskTrackDbContext _context;

        public IncidentsController(RiskTrackDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] CreateIncidentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == dto.AssetId);
            if (asset == null)
                return NotFound($"AssetId {dto.AssetId} not found.");

            var incident = new Incident
            {
                AssetId = dto.AssetId,
                IncidentDate = dto.IncidentDate,
                Description = dto.Description,
                ResolutionDurationHours = dto.ResolutionDurationHours,
                ImpactDurationMinutes = dto.ImpactDurationMinutes
            };

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Incident saved successfully",
                IncidentId = incident.IncidentId
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound($"Incident with ID {id} not found.");

            return Ok(incident);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncident(int id, [FromBody] CreateIncidentDto dto)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound($"Incident with ID {id} not found.");

            incident.AssetId = dto.AssetId;
            incident.IncidentDate = dto.IncidentDate;
            incident.Description = dto.Description;
            incident.ResolutionDurationHours = dto.ResolutionDurationHours;
            incident.ImpactDurationMinutes = dto.ImpactDurationMinutes;

            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Incident updated successfully", IncidentId = incident.IncidentId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound($"Incident with ID {id} not found.");

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Incident deleted successfully", IncidentId = id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncidents()
        {
            var incidents = await _context.Incidents.ToListAsync();
            return Ok(incidents);
        }


    }

    public class CreateIncidentDto
    {
        public string AssetId { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; }
        public int? ResolutionDurationHours { get; set; }
        public int? ImpactDurationMinutes { get; set; }
    }
}

