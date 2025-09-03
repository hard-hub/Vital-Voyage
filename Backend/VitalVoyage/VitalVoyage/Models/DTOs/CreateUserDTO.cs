namespace VitalVoyage.Models.DTOs
{
    public class CreateUserDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
