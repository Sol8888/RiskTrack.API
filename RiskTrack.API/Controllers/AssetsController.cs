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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(string id)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == id);
            if (asset == null) return NotFound("Asset not found");
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] Asset asset)
        {
            asset.CreatedAt = DateTime.UtcNow;
            asset.UpdatedAt = DateTime.UtcNow;
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAssetById), new { id = asset.AssetId }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] Asset updatedAsset)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Asset not found");

            
            asset.Name = updatedAsset.Name;
            asset.AssetTypeId = updatedAsset.AssetTypeId;
            asset.CompanyId = updatedAsset.CompanyId;
            asset.OwnerTeamId = updatedAsset.OwnerTeamId;
            asset.ContainsPII = updatedAsset.ContainsPII;
            asset.DataSource = updatedAsset.DataSource;
            asset.RevenuePerMinuteUsd = updatedAsset.RevenuePerMinuteUsd;
            asset.CriticalFlowPercentage = updatedAsset.CriticalFlowPercentage;
            asset.TotalPiiRecords = updatedAsset.TotalPiiRecords;
            asset.AnnualLicenseCostUsd = updatedAsset.AnnualLicenseCostUsd;
            asset.AnnualSupportHours = updatedAsset.AnnualSupportHours;
            asset.EngineerHourlyRateUsd = updatedAsset.EngineerHourlyRateUsd;
            asset.MonthlyDowntimeMin = updatedAsset.MonthlyDowntimeMin; 
            asset.AnnualCriticalVulnerabilities = updatedAsset.AnnualCriticalVulnerabilities;
            asset.DataCorruptionErrors = updatedAsset.DataCorruptionErrors;
            asset.UpdatedAt = DateTime.UtcNow;
            asset.DecidedRiskUsd = updatedAsset.DecidedRiskUsd;

            await _context.SaveChangesAsync();
            return Ok(asset);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Asset not found");

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
