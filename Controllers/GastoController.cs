using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Gastos;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastoController : ControllerBase
    {
        private readonly IGastoService _gastoService;

        public GastoController(IGastoService gastoService)
        {
            _gastoService = gastoService;
        }

        // GET: api/Gasto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var gastos = await _gastoService.ObtenerTodosAsync();
            return Ok(gastos);
        }

        // GET: api/Gasto/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null)
                return NotFound(new { mensaje = $"No se encontró el gasto con ID {id}" });

            return Ok(gasto);
        }

       

        // POST: api/Gasto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearGastoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _gastoService.CrearGastoAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el gasto." });

            return StatusCode(201, new { mensaje = "Gasto creado con éxito" });
        }

        // PUT: api/Gasto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarGastoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _gastoService.ActualizarGastoAsync(id, dto);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el gasto con ID {id}" });

            return Ok(new { mensaje = "Gasto actualizado con éxito" });
        }

        // DELETE: api/Gasto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _gastoService.EliminarGastoAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el gasto con ID {id}" });

            return Ok(new { mensaje = "Gasto eliminado con éxito" });
        }
    }
}