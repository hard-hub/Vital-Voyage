using VitalVoyage.Models.DTOs;

namespace VitalVoyage.Services.Contracts
{
    public interface IEmailServices
    {
        Task SendEmail(EmailDTO verifyEmailDTO);
    }
}
