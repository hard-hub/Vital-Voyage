using VitalVoyage.Models.Entities;

namespace VitalVoyage.Services.Contracts
{
    public interface IJwtServices
    {
        string GenerateJwtToken(User user);
    }
}
