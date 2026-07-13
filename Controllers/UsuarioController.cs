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

       
        [HttpGet("todos")]
        public async Task<IActionResult> GetTodos()
        {
            return Ok(await _usuarioService.ObtenerTodosAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioDto dto)
        {
            var exito = await _usuarioService.ActualizarUsuarioAsync(id, dto);

            if (!exito)
                return BadRequest("No se pudo actualizar el usuario o vincular el empleado.");

            return NoContent();
        }

        [HttpPut("{id}/aprobar")]
        public async Task<IActionResult> Aprobar(int id, [FromBody] AprobarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Rol))
            {
                return BadRequest(new { mensaje = "El rol es obligatorio." });
            }

            var resultado = await _usuarioService.AprobarUsuarioAsync(id, dto.Rol, dto.IdEmpleado);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = $"Usuario aprobado con éxito como {dto.Rol}" });
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> Rechazar(int id)
        {
            var resultado = await _usuarioService.RechazarUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario rechazado" });
        }

        [HttpPut("{id}/suspender")]
        public async Task<IActionResult> Suspender(int id)
        {
            var resultado = await _usuarioService.SuspenderUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario suspendido" });
        }

        [HttpPut("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var resultado = await _usuarioService.ReactivarUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario reactivado" });
        }

        [HttpPut("{id}/rol")]
        public async Task<IActionResult> CambiarRol(int id, [FromBody] CambiarRolDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Rol))
            {
                return BadRequest(new { mensaje = "El rol es obligatorio." });
            }

            var resultado = await _usuarioService.CambiarRolAsync(id, dto.Rol);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = $"Rol actualizado a {dto.Rol}" });
        }

        // ✅ NUEVO: Eliminar usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}" });

            return Ok(new { mensaje = "Usuario eliminado correctamente." });
        }
    }
}