namespace RiskTrack.API.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Leader { get; set; }
        public string? ContactEmail { get; set; }
    }
}
