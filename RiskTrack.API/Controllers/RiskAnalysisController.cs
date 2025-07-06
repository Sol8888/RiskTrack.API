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
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == request.AssetId);
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

            _context.RiskAnalysis.Add(analysis);
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

        [HttpGet("full/{assetId}")]
        public async Task<IActionResult> GetFullRiskAnalysis(string assetId)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == assetId);
            if (asset == null) return NotFound("Asset not found");

            var lefCount = await _context.Incidents
                .Where(i => i.AssetId == asset.AssetId && i.IncidentDate >= DateTime.UtcNow.AddYears(-1))
                .CountAsync();

            var lef = _frequencyService.CalculateLEF(lefCount);

            var vna = _riskService.CalculateVNA(asset);
            var si = _riskService.CalculateSI(asset);

            
            var pd = _lossService.CalculatePD(asset.RevenuePerMinuteUsd ?? 0, 60); 
            var pi = _lossService.CalculatePI(pd);
            var cr = _lossService.CalculateCR(4, asset.EngineerHourlyRateUsd ?? 0); 
            var dpf = _lossService.CalculateDPF(asset.TotalPiiRecords ?? 0, 10); 
            var dirf = 0;
            var ces = _lossService.CalculateCES(asset.TotalPiiRecords ?? 0, 10); 

            var lm = _lossService.CalculateLM(pd, pi, cr, dpf, dirf, ces);
            var lmTipico = pd + pi + cr;
            var ale = _riskService.CalculateALE(lef, lmTipico);

            var riesgoAceptado = ale <= asset.DecidedRiskUsd;

            return Ok(new
            {
                asset.AssetId,
                asset.Name,
                asset.ContainsPII,
                asset.RevenuePerMinuteUsd,
                asset.CriticalFlowPercentage,
                asset.TotalPiiRecords,
                asset.AnnualLicenseCostUsd,
                asset.AnnualSupportHours,
                asset.EngineerHourlyRateUsd,
                asset.MonthlyDowntimeMin,
                asset.AnnualCriticalVulnerabilities,
                asset.DataCorruptionErrors,
                asset.DecidedRiskUsd,

                VNA = vna,
                SI = si,
                LEF = lef,
                PD = pd,
                PI = pi,
                CR = cr,
                CES = ces,
                DPF = dpf,
                DIRF = dirf,
                LM = lm,
                LM_Tipico = lmTipico,
                ALE = ale,
                UmbralAceptable = asset.DecidedRiskUsd,
                RiesgoAceptado = riesgoAceptado,
                Message = "Full risk analysis summary"
            });
        }

        [HttpPost("treatment")]
        public async Task<IActionResult> CalculateRiskTreatment([FromBody] RiskTreatmentRequest request)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == request.AssetId);
            if (asset == null) return NotFound("Asset not found");

            var lefCount = await _context.Incidents
                .Where(i => i.AssetId == asset.AssetId && i.IncidentDate >= DateTime.UtcNow.AddYears(-1))
                .CountAsync();

            var lef = _frequencyService.CalculateLEF(lefCount);

            // Cálculo del LM típico (sin control)
            var pd = _lossService.CalculatePD(asset.RevenuePerMinuteUsd ?? 0, 60);
            var pi = _lossService.CalculatePI(pd);
            var cr = _lossService.CalculateCR(4, asset.EngineerHourlyRateUsd ?? 0);
            var lmTipico = pd + pi + cr;

            var aleInicial = _riskService.CalculateALE(lef, lmTipico);

            // Aplicación de control
            var reductionFactor = request.EstimatedReductionFactor;
            var lmResidual = lmTipico * (1 - reductionFactor);
            var aleResidual = _riskService.CalculateALE(lef, lmResidual);

            // Control cost
            var controlCost = request.ControlCost;

            var gananciaNeta = (aleInicial - aleResidual) - controlCost;

            // Calcular ROSI
            var rosi = _riskService.CalculateROSI(aleInicial, aleResidual, controlCost);

            // Evaluar contra el umbral aceptable
            //bool riesgoAceptado = aleInicial <= asset.DecidedRiskUsd;

            return Ok(new
            {
                AssetId = asset.AssetId,
                ALE_Inicial = aleInicial,
                ALE_Residual = aleResidual,
                ControlCost = controlCost,
                GananciaNeta = gananciaNeta,
                ROSI = rosi,
                //RiesgoAceptado = riesgoAceptado,
                //UmbralAceptable = asset.DecidedRiskUsd,
                Message = "Tratamiento del riesgo calculado"
            });
        }



    }

    public class RiskAnalysisRequest
    {
        public string AssetId { get; set; } = default!;
        public int IncidentDurationMinutes { get; set; }
        public int ResponseHours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal PenaltyPerRecord { get; set; }
        public int ErrorCount { get; set; }
        public decimal ReworkCost { get; set; }
        public decimal InitialAle { get; set; }
        public decimal ControlCost { get; set; }
    }

    public class RiskTreatmentRequest
    {
        public string AssetId { get; set; } = default!;
        public decimal ControlCost { get; set; }
        public decimal EstimatedReductionFactor { get; set; } 
    }
}

