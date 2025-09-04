using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalVoyage.Models;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Models.Entities;
using VitalVoyage.Services.Contracts;

namespace VitalVoyage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GetUserDTO>>> CreateUser([FromBody]CreateUserDTO createUserDTO)
        {
            var response = await _userServices.CreateUser(createUserDTO);
            return StatusCode(response.SuccessCode, response);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse>> DeleteUser([FromRoute]Guid id)
        {
            var response = await _userServices.DeleteUser(id);
            return StatusCode(response.SuccessCode,response);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetUserDTO>>>> GetAllUsers()
        {
           var response = await _userServices.GetAllUsers();
            return StatusCode(response.SuccessCode, response);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<GetUserDTO>>> GetUserById([FromRoute]Guid id)
        {
            var response = await _userServices.GetUserById(id);
            return StatusCode(response.SuccessCode, response);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<GetUserDTO>>> UpdateUser([FromRoute]Guid id, [FromBody]UpdateUserDTO updateUserDTO)
        {
            var response = await _userServices.UpdateUser(id, updateUserDTO);
            return StatusCode(response.SuccessCode, response);
        }
    }
}
