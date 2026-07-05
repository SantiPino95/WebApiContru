using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Usuarios;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            return Ok(await _usuarioService.ObtenerPendientesAsync());
        }

        [HttpPut("{id}/aprobar")]
        public async Task<IActionResult> Aprobar(int id)
        {
            var resultado = await _usuarioService.AprobarUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario aprobado con éxito" });
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> Rechazar(int id)
        {
            var resultado = await _usuarioService.RechazarUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario rechazado" });
        }
    }
}