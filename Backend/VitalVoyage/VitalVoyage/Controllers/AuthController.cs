using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Models.Entities;
using VitalVoyage.Services;
using VitalVoyage.Services.Contracts;

namespace VitalVoyage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> UserLogin(UserLoginDTO loginDTO)
        {
            var response = await _authServices.UserLogin(loginDTO);
            return StatusCode(response.SuccessCode, response);
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserResponseDTO>>> UserRegister(UserRegisterDTO registerDTO)
        {
            var response = await _authServices.UserRegister(registerDTO);
            return StatusCode(response.SuccessCode, response);
        }
    }
}
