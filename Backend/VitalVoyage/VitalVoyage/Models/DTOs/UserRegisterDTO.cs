using VitalVoyage.Models.Entities;

namespace VitalVoyage.Models.DTOs
{
    public class UserRegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public Roles Role { get; set; } = Roles.Patient;
    }
}
