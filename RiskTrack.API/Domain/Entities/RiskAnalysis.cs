namespace RiskTrack.API.Domain.Entities
{
    public class RiskAnalysis
    {
        public int RiskAnalysisId { get; set; }
        public string? AssetId { get; set; } = default!;
        public DateTime? AnalysisDate { get; set; }
        public decimal? VnaValue { get; set; }
        public decimal? SiValue { get; set; }
        public decimal? LefValue { get; set; }
        public decimal? LmValue { get; set; }
        public decimal? AleValue { get; set; }
        public string? AnalystNotes { get; set; }
    }
}
