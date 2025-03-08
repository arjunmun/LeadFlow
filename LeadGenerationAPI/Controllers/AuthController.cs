using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeadGenerationAPI.Services;
using LeadGenerationAPI.Models.DTOs;

namespace LeadGenerationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.AuthenticateAsync(model.Username, model.Password);
            if (!success)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.RegisterAsync(model.Username, model.Password);
            if (!success)
                return BadRequest(new { message = "Username already exists" });

            return Ok(new { message = "Registration successful" });
        }
    }
} 