namespace RiskTrack.API.RiskTrack.Domain.Entities
{
    public class RiskAnalysis
    {
        public int RiskAnalysisId { get; set; }
        public int? AssetId { get; set; }
        public DateTime? AnalysisDate { get; set; }
        public decimal? VnaValue { get; set; }
        public decimal? SiValue { get; set; }
        public decimal? LefValue { get; set; }
        public decimal? LmValue { get; set; }
        public decimal? AleValue { get; set; }
        public string? AnalystNotes { get; set; }
    }
}
