namespace RiskTrack.API.Domain.Entities
{
    public class AssetControl
    {
        public int AssetControlId { get; set; }
        public string? AssetId { get; set; }
        public int? ControlMeasureId { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? Status { get; set; }
    }
}
