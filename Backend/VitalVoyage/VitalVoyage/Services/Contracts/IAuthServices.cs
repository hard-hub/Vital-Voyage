using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;

namespace VitalVoyage.Services.Contracts
{
    public interface IAuthServices
    {
        Task<ApiResponse<UserResponseDTO>> UserRegister(UserRegisterDTO registerDTO);
        Task<ApiResponse<string>> UserLogin(UserLoginDTO loginDTO);
    }
}
