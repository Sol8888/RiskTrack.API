using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RiskTrack.API.RiskTrack.Domain.Entities
{
    public class Incident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IncidentId { get; set; }
        public string? AssetId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string? Description { get; set; }
        public int? ResolutionDurationHours { get; set; }
        public int? ImpactDurationMinutes { get; set; }
    }
}
