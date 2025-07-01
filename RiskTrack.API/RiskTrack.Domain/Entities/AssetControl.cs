namespace RiskTrack.API.RiskTrack.Domain.Entities
{
    public class AssetControl
    {
        public int AssetControlId { get; set; }
        public int? AssetId { get; set; }
        public int? ControlMeasureId { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? Status { get; set; }
    }
}
