using Microsoft.AspNetCore.Mvc;
using MiWebApi.Dtos;
using MiWebApi.Services.Herramientas;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HerramientaController : ControllerBase
    {
        private readonly IHerramientaService _herramientaService;

        public HerramientaController(IHerramientaService herramientaService)
        {
            _herramientaService = herramientaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HerramientaListadoDto>>> ObtenerTodas()
        {
            var herramientas = await _herramientaService.ObtenerHerramientasParaAdminAsync();
            return Ok(herramientas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HerramientaListadoDto>> ObtenerPorId(int id)
        {
            var herramienta = await _herramientaService.ObtenerHerramientaPorIdAsync(id);
            if (herramienta == null)
                return NotFound($"No se encontró la herramienta con ID {id}.");
            return Ok(herramienta);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearHerramientaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _herramientaService.AgregarHerramientaAsync(dto);
            if (!resultado)
                return BadRequest("No se pudo registrar la herramienta.");

            return StatusCode(201, "Herramienta registrada con éxito.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CrearHerramientaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _herramientaService.ActualizarHerramientaAsync(id, dto);
            if (!resultado)
                return NotFound($"No se pudo actualizar. No existe la herramienta con ID {id}.");

            return Ok("Herramienta actualizada con éxito.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _herramientaService.EliminarHerramientaAsync(id);
            if (!resultado)
                return BadRequest("No se puede eliminar la herramienta porque está asignada a una obra.");

            return Ok("Herramienta eliminada con éxito.");
        }

        // ✅ ASIGNAR - CORREGIDO
        [HttpPost("asignar-salida")]
        public async Task<IActionResult> RegistrarSalida(int idHerramienta, int idObra)
        {
            var resultado = await _herramientaService.RegistrarSalidaAsignacionAsync(idHerramienta, idObra);

            if (!resultado)
                return BadRequest("No se pudo registrar la salida de la herramienta. Verifica que no esté ya asignada.");

            return Ok("Salida de herramienta registrada con éxito.");
        }

        // ✅ DEVOLVER - CORREGIDO
        [HttpPost("registrar-devolucion")]
        public async Task<IActionResult> RegistrarDevolucion(int idHerramienta, int idObra)
        {
            var resultado = await _herramientaService.RegistrarDevolucionHerramientaAsync(idHerramienta, idObra);

            if (!resultado)
                return BadRequest("No se encontró una asignación activa para esa herramienta y obra, o no se pudo procesar.");

            return Ok("Devolución procesada y herramienta marcada como Disponible.");
        }
    }
}