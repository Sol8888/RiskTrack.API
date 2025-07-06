using System.ComponentModel.DataAnnotations;

namespace RiskTrack.API.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required]
        [RegularExpression("^(A|U)$", ErrorMessage = "Role must be 'A' or 'U'")]
        public string? Role { get; set; }
    }
}
