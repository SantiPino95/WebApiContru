using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Seguimientos;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguimientoController : ControllerBase
    {
        private readonly ISeguimientoService _seguimientoService;

        public SeguimientoController(ISeguimientoService seguimientoService)
        {
            _seguimientoService = seguimientoService;
        }

        // 1. GET: api/seguimiento/obra/5 (Lista todo el historial de avances de una obra)
        [HttpGet("obra/{idObra}")]
        public async Task<IActionResult> GetByObra(int idObra)
        {
            var historial = await _seguimientoService.ListarPorObraAsync(idObra);
            return Ok(historial);
        }

        // 2. GET: api/seguimiento/obra/5/ultimo (Obtiene solo el último avance registrado)
        [HttpGet("obra/{idObra}/ultimo")]
        public async Task<IActionResult> GetUltimoByObra(int idObra)
        {
            var ultimo = await _seguimientoService.ObtenerUltimoPorObraAsync(idObra);
            if (ultimo == null)
            {
                return NotFound(new { mensaje = "No hay registros de seguimiento para esta obra." });
            }
            return Ok(ultimo);
        }

        // 3. POST: api/seguimiento (Registra un nuevo avance de obra)
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] CrearSeguimientoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🚀 Pasamos el DTO completo al servicio
            var (exito, mensaje) = await _seguimientoService.RegistrarAvanceAsync(dto);

            if (!exito)
            {
                // Si la suma supera el 100% o la obra no existe, devuelve HTTP 400 Bad Request
                return BadRequest(new { mensaje });
            }

            return Ok(new { mensaje });
        }
    }
}