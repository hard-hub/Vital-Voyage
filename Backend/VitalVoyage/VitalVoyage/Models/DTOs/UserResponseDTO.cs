using VitalVoyage.Models.Entities;

namespace VitalVoyage.Models.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public Roles Role { get; set; }
    }
}
