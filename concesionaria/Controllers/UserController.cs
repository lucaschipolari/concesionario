using Concesionario.Application.Dto.Core;
using Concesionario.Application.Interfaces;
using Concesionario.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto request)
        {
            try
            {
                var result = await _userService.AddUser(request);
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("employees/{branchId}")]
        public async Task<IActionResult> GetEmployeesByBranchId(Guid branchId)
        {
            try
            {
                var employees = await _userService.GetEmployeesByBranchId(branchId);
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
