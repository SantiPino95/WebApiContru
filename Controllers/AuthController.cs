using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Auth;

namespace MiWebApi.Controllers
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

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RegisterAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "El email ya está registrado o la cédula ya existe." });

            return StatusCode(201, new { mensaje = "Usuario registrado con éxito. Esperando aprobación del administrador." });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.LoginAsync(dto);
            if (resultado == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas o usuario no activo." });

            return Ok(resultado);
        }
    }
}
