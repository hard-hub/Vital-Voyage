using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Models.Entities;

namespace VitalVoyage.Services.Contracts
{
    public interface IUserServices
    {
        Task<ApiResponse<IEnumerable<GetUserDTO>>> GetAllUsers();
        Task<ApiResponse<GetUserDTO>> GetUserById(Guid id);
        Task<ApiResponse<GetUserDTO>> CreateUser(CreateUserDTO user);
        Task<ApiResponse<GetUserDTO>> UpdateUser(Guid id, UpdateUserDTO user);
        Task<ApiResponse> DeleteUser(Guid id); 
    }
}
