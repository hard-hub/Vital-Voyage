namespace VitalVoyage.Models.DTOs
{
    public class GetUserDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
