using Azure.Identity;
using Concesionario.Application.Dto;
using Concesionario.Application.Interfaces;
using Concesionario.Application.Services;
using Concesionario.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public AuthenticateController(UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationUserIdentity> signInManager,
            JwtTokenService jwtTokenService,IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            var identityUser = await _userManager.FindByNameAsync(request.Username);
            if (identityUser == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            // Buscar el usuario en tu tabla de negocio (Customer o Employee)
            var appUser = await _userService.GetUserByName(request.Username); // o por username si lo preferís

            if (appUser == null)
            {
                return Unauthorized("Usuario no registrado en el sistema");
            }

            var token = _jwtTokenService.GenerateToken(appUser.Email, appUser.userId); // 👈 este es el GUID correcto

            return Ok(new { token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel request)
        {


            var user = new ApplicationUserIdentity { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Opcional: enviar email de confirmación, etc.
            return Ok("Usuario registrado correctamente.");
        }

    }
}
