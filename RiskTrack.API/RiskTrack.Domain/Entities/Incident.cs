namespace RiskTrack.API.RiskTrack.Domain.Entities
{
    public class Incident
    {
        public int IncidentId { get; set; }
        public int? AssetId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string? Description { get; set; }
        public int? ResolutionDurationHours { get; set; }
        public int? ImpactDurationMinutes { get; set; }
    }
}
