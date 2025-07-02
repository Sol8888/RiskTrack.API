using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskTrack.API.RiskTrack.Application.Services;
using RiskTrack.API.RiskTrack.Domain.Entities;
using RiskTrack.Infrastructure.Data;

namespace RiskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskAnalysisController : ControllerBase
    {
        private readonly RiskTrackDbContext _context;
        private readonly RiskCalculationService _riskService;
        private readonly LossEstimationService _lossService;
        private readonly FrequencyService _frequencyService;

        public RiskAnalysisController(RiskTrackDbContext context)
        {
            _context = context;
            _riskService = new RiskCalculationService();
            _lossService = new LossEstimationService();
            _frequencyService = new FrequencyService();
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateRiskAnalysis([FromBody] RiskAnalysisRequest request)
        {
            var asset = await _context.Assets.FindAsync(request.AssetId);
            if (asset == null) return NotFound("Asset not found");

            
            var lefCount = await _context.Incidents
                .Where(i => i.AssetId == asset.AssetId && i.IncidentDate >= DateTime.UtcNow.AddYears(-1))
                .CountAsync();

            var lef = _frequencyService.CalculateLEF(lefCount);

            
            var vna = _riskService.CalculateVNA(asset);
            var si = _riskService.CalculateSI(asset);

            
            var pd = _lossService.CalculatePD(asset.RevenuePerMinuteUsd ?? 0, request.IncidentDurationMinutes);
            var pi = _lossService.CalculatePI(pd);
            var cr = _lossService.CalculateCR(request.ResponseHours, request.HourlyRate);
            var dpf = _lossService.CalculateDPF(asset.TotalPiiRecords ?? 0, request.PenaltyPerRecord);
            var dirf = _lossService.CalculateDIRF(request.ErrorCount, request.ReworkCost);
            var ces = _lossService.CalculateCES(asset.TotalPiiRecords ?? 0, request.PenaltyPerRecord);

            var lm = _lossService.CalculateLM(pd, pi, cr, dpf, dirf, ces);
            var ale = _riskService.CalculateALE(lef, lm);
            var rosi = _riskService.CalculateROSI(request.InitialAle, ale, request.ControlCost);

            
            var analysis = new RiskAnalysis
            {
                AssetId = asset.AssetId,
                AnalysisDate = DateTime.UtcNow,
                VnaValue = vna,
                SiValue = si,
                LefValue = lef,
                LmValue = lm,
                AleValue = ale,
                AnalystNotes = $"ROSI: {rosi:N2}%"
            };

            _context.RiskAnalyses.Add(analysis);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AssetId = asset.AssetId,
                VNA = vna,
                SI = si,
                LEF = lef,
                LM = lm,
                ALE = ale,
                ROSI = rosi,
                Message = "Risk analysis calculated and saved"
            });
        }
    }

    public class RiskAnalysisRequest
    {
        public int AssetId { get; set; }
        public int IncidentDurationMinutes { get; set; }
        public int ResponseHours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal PenaltyPerRecord { get; set; }
        public int ErrorCount { get; set; }
        public decimal ReworkCost { get; set; }
        public decimal InitialAle { get; set; }
        public decimal ControlCost { get; set; }
    }

}

