using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VitalVoyage.DatabaseContext;
using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Models.Entities;
using VitalVoyage.Services.Contracts;

namespace VitalVoyage.Services
{
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserServices> _logger;
        private readonly IMapper _mapper;
        public UserServices(AppDbContext context, ILogger<UserServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ApiResponse<GetUserDTO>> CreateUser(CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("Creating a new user");

            // check if email already exists
            if(await _context.Users.AnyAsync(u => u.Email == createUserDTO.Email))
            {
                _logger.LogWarning("Email {Email} already exists", createUserDTO.Email);
                return ApiResponse<GetUserDTO>.FailureResponse(400, "Email already exists");
            }
            // Map DTO to Entity
            var user = _mapper.Map<User>(createUserDTO);

            // Set additional fields
            user.Id = Guid.NewGuid();
            user.CreatedOn = DateTime.UtcNow;
            user.CreatedBy = user.Id; // or fetch from context
            user.IsActive = true;
            user.Role = Roles.Patient; // default role
            user.IsEmailVerified = false;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpires = null; // set as needed

            // Save to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User created successfully with ID {UserId}", user.Id);

            var userDTO = _mapper.Map<GetUserDTO>(user);

            return ApiResponse<GetUserDTO>.SuccessResponse(userDTO, 201, "User created successfully");
        }

        public async Task<ApiResponse> DeleteUser(Guid id)
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);

            var user = await _context.FindAsync<User>(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return ApiResponse.FailureResponse(404, "User not found");
            }
            user.IsActive = false;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = id; // or fetch from context
            await _context.SaveChangesAsync();  

            _logger.LogInformation("User with ID {UserId} deleted successfully", id);
            return ApiResponse.SuccessResponse("User deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetUserDTO>>> GetAllUsers()
        {
            _logger.LogInformation("Retrieving all users");
        
            var users = await _context.Users.Where(u => u.IsActive).ToListAsync();
            var userDTOs = _mapper.Map<IEnumerable<GetUserDTO>>(users);

            _logger.LogInformation("Retrieved {UserCount} users", userDTOs.Count());
            return ApiResponse<IEnumerable<GetUserDTO>>.SuccessResponse(userDTOs, 200, "Users retrieved successfully");
        }

        public async Task<ApiResponse<GetUserDTO>> GetUserById(Guid id)
        {
            _logger.LogInformation("Retrieving user with ID {UserId}", id);

            var user = await _context.FindAsync<User>(id);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return ApiResponse<GetUserDTO>.FailureResponse(404, "User not found");
            }
            var userDTO = _mapper.Map<GetUserDTO>(user);

            _logger.LogInformation("User with ID {UserId} retrieved successfully", id);

            return ApiResponse<GetUserDTO>.SuccessResponse(userDTO, 200, "User retrieved successfully");
        }

        public async Task<ApiResponse<GetUserDTO>> UpdateUser(Guid id, UpdateUserDTO updateUserDTO)
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);

            var user = await _context.FindAsync<User>(id);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return ApiResponse<GetUserDTO>.FailureResponse(404, "User not found");
            }
            _mapper.Map(updateUserDTO, user);
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = id; // or fetch from context

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User with ID {UserId} updated successfully", id);

            var userDTO = _mapper.Map<GetUserDTO>(user);

            return ApiResponse<GetUserDTO>.SuccessResponse(userDTO, 200, "User updated successfully");
        }
    }
}
