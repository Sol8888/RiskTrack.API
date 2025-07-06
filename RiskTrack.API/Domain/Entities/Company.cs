namespace RiskTrack.API.Domain.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? RUC { get; set; }
        public string? Sector { get; set; }
    }
}
