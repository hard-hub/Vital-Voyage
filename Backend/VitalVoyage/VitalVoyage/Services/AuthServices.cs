using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using VitalVoyage.DatabaseContext;
using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Models.Entities;
using VitalVoyage.Services.Contracts;

namespace VitalVoyage.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthServices> _logger;
        private readonly IJwtServices _jwtServices;
        private readonly IEmailServices _emailServices;
        private readonly IMapper _mapper;
        public AuthServices(AppDbContext appDbContext, ILogger<AuthServices> logger, IJwtServices jwtServices, IMapper mapper, IEmailServices emailServices)
        {
            _context = appDbContext;
            _logger = logger;
            _jwtServices = jwtServices;
            _mapper = mapper;
            _emailServices = emailServices;
        }
        public async Task<ApiResponse<string>> UserLogin(UserLoginDTO loginDTO)
        {
            _logger.LogInformation("Attempting Login for Email: {Email}", loginDTO.Email);

            // check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed. Email not found: {Email}", loginDTO.Email);
                return ApiResponse<string>.FailureResponse(400, "Invalid Credentials");
            }

            // check if the password is correct
            bool isPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
            if(!isPassword)
            {
                _logger.LogWarning("Login failed. Invalid password for Email : {Email}", loginDTO.Email);
                return ApiResponse<string>.FailureResponse(400, "Invalid login attempt");
            }

            // check if the email is verified
            if(!user.IsEmailVerified)
            {
                _logger.LogWarning("Login blocked for unverified user : {UserId}", user.Id);
                // TODO: Resend verification email
                return ApiResponse<string>.FailureResponse(400, "Unverified Email");
            }

            // generate token
            _logger.LogInformation("Login Successful for the user");
            var token = _jwtServices.GenerateJwtToken(user);
            return ApiResponse<string>.SuccessResponse(token, 200, "Login Successful");
        }

        public async Task<ApiResponse<UserResponseDTO>> UserRegister(UserRegisterDTO registerDTO)
        {
            _logger.LogInformation("Starting registeraton for email: {Email}", registerDTO.Email);

            // check if the email already exists
            if(await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
            {
                _logger.LogWarning("Registeration failed. Email already exists: {Email}", registerDTO.Email);

                return ApiResponse<UserResponseDTO>.FailureResponse(400, "User with this email already exists", null);
            }

            // generate verification 
            var verificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            var tokenHash = BCrypt.Net.BCrypt.HashPassword(verificationToken);
            var tokenExpires = DateTime.UtcNow.AddMinutes(60);

            // map user with dto
            var user = _mapper.Map<User>(registerDTO);
            // add additional features
            user.Id = new Guid();
            user.IsEmailVerified = false;
            user.IsActive = true;
            user.CreatedBy = user.Id;
            user.CreatedOn = DateTime.UtcNow;
            user.EmailVerificationToken = tokenHash;
            user.EmailVerificationTokenExpires = tokenExpires;

            // save user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User registered successfully with ID : {userId}", user.Id);

            // send email with verification token
            _logger.LogInformation("Sent verification email to the user.");
            var resetLink = $"http://localhost:4200/verify-email?token={verificationToken}";
            var body = $@"
            <p>We received a request to reset your password. Click the link below to set a new password. This link will expire in 15 minutes.</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>If you did not request a password reset, please ignore this email.</p>
            <p>Thank you,<br/>Social Compass Team</p>";
            EmailDTO verifyEmailDTO = new EmailDTO
            {
                To = user.Email,
                Subject = "Email Verification",
                Body = body
            };
            Console.WriteLine(verifyEmailDTO.To);
            await _emailServices.SendEmail(verifyEmailDTO);

            var userResponseDTO = _mapper.Map<UserResponseDTO>(user);
            return ApiResponse<UserResponseDTO>.SuccessResponse(userResponseDTO, 200, "User registered successfully. Please check your email to verify the account.");  
        }
    }
}
