using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskTrack.API.RiskTrack.Application.Services;
using RiskTrack.API.RiskTrack.Domain.Entities;
using RiskTrack.Infrastructure.Data;



namespace RiskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly RiskTrackDbContext _context;
        private readonly RiskCalculationService _riskService;

        public AssetsController(RiskTrackDbContext context)
        {
            _context = context;
            _riskService = new RiskCalculationService();
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _context.Assets.ToListAsync();
            return Ok(assets);
        }

        
        [HttpGet("vna/{id}")]
        public async Task<IActionResult> CalculateVNA(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return NotFound("Asset not found");

            var vna = _riskService.CalculateVNA(asset);
            return Ok(new { AssetId = id, VNA = vna });
        }

        
        [HttpGet("si/{id}")]
        public async Task<IActionResult> CalculateSI(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return NotFound("Asset not found");

            var si = _riskService.CalculateSI(asset);
            return Ok(new { AssetId = id, SI = si });
        }

        
        [HttpGet("ale/{id}")]
        public async Task<IActionResult> CalculateALE(int id, [FromQuery] decimal lef, [FromQuery] decimal lm)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return NotFound("Asset not found");

            var ale = _riskService.CalculateALE(lef, lm);
            return Ok(new { AssetId = id, LEF = lef, LM = lm, ALE = ale });
        }
    }
}
