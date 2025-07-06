using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RiskTrack.API.RiskTrack.Domain.Entities
{
    public class Asset
    {
        [Key]
        [JsonIgnore]
        public int AssetCod { get; set; }
        public string? AssetId { get; set; } = default!;
        public string? Name { get; set; }
        public int? AssetTypeId { get; set; }
        public int? CompanyId { get; set; }
        public int? OwnerTeamId { get; set; }
        public bool? ContainsPII { get; set; }
        public string? DataSource { get; set; }
        public decimal? RevenuePerMinuteUsd { get; set; }
        public decimal? CriticalFlowPercentage { get; set; }
        public long? TotalPiiRecords { get; set; }
        public decimal? AnnualLicenseCostUsd { get; set; }
        public int? AnnualSupportHours { get; set; }
        public decimal? EngineerHourlyRateUsd { get; set; }
        public int? MonthlyDowntimeMin { get; set; }
        public int? AnnualCriticalVulnerabilities { get; set; }
        public int? DataCorruptionErrors { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public decimal? DecidedRiskUsd { get; set; }

    }
}
