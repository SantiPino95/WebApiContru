using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.RegistroHoras;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL será: api/RegistroHora
    public class RegistroHoraController : ControllerBase
    {
        private readonly IRegistroHoraService _registroHoraService;

        public RegistroHoraController(IRegistroHoraService registroHoraService)
        {
            _registroHoraService = registroHoraService;
        }

        // GET: api/RegistroHora/empleado/5?anio=2026&mes=7
        [HttpGet("empleado/{idEmpleado}")]
        public async Task<IActionResult> ObtenerPorEmpleadoYMes(int idEmpleado, [FromQuery] int anio, [FromQuery] int mes)
        {
            if (anio == 0 || mes is < 1 or > 12)
            {
                return BadRequest(new { mensaje = "Año y mes son obligatorios (mes entre 1 y 12)." });
            }

            var registros = await _registroHoraService.ObtenerPorEmpleadoYMesAsync(idEmpleado, anio, mes);
            return Ok(registros);
        }

        // GET: api/RegistroHora/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var registro = await _registroHoraService.ObtenerPorIdAsync(id);
            if (registro == null)
            {
                return NotFound(new { mensaje = $"No se encontró el registro con ID {id}" });
            }
            return Ok(registro);
        }

        // POST: api/RegistroHora/empleado/5
        [HttpPost("empleado/{idEmpleado}")]
        public async Task<IActionResult> Crear(int idEmpleado, [FromBody] CrearRegistroHoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (exito, error, creado) = await _registroHoraService.CrearAsync(idEmpleado, dto);
            if (!exito)
            {
                return BadRequest(new { mensaje = error });
            }

            return StatusCode(201, creado);
        }

        // PUT: api/RegistroHora/5/empleado/8
        [HttpPut("{id}/empleado/{idEmpleado}")]
        public async Task<IActionResult> Actualizar(int id, int idEmpleado, [FromBody] ActualizarRegistroHoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (exito, error) = await _registroHoraService.ActualizarAsync(idEmpleado, id, dto);
            if (!exito)
            {
                return BadRequest(new { mensaje = error });
            }

            return Ok(new { mensaje = "Registro actualizado con éxito" });
        }
    }
}