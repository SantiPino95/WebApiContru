using Microsoft.AspNetCore.Mvc;
using MiWebApi.Services.Presupuestos;
using MiWebApi.DTOs;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestosController : ControllerBase
    {
        private readonly IPresupuestoService _service;
        public PresupuestosController(IPresupuestoService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ObtenerPresupuestosAsync());

        [HttpPost]
        public async Task<IActionResult> Post(CrearPresupuestoDto dto)
        {
            var res = await _service.CrearPresupuestoAsync(dto);
            return res ? Ok("Presupuesto cargado con éxito con sus renglones calculados.") : BadRequest("No se pudo procesar el presupuesto.");
        }


        // PUT: api/Presupuestos/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                return BadRequest(new { mensaje = "El estado es obligatorio." });

            var resultado = await _service.CambiarEstadoAsync(id, nuevoEstado);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el presupuesto con ID {id} o el estado no es válido." });

            return Ok(new { mensaje = $"Presupuesto actualizado a '{nuevoEstado}' con éxito" });
        }
    }
}

