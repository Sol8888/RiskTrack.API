namespace RiskTrack.API.Domain.Entities
{
    public class ControlMeasure
    {
        public int ControlMeasureId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? AnnualCostUsd { get; set; }
        public int? ImplementationHours { get; set; }
        public decimal? EstimatedFrequencyReduction { get; set; }
        public decimal? EstimatedMagnitudeReduction { get; set; }
    }
}
