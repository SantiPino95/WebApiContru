using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Novedades;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NovedadesController : ControllerBase
    {
        private readonly INovedadService _novedadService;

        public NovedadesController(INovedadService novedadService)
        {
            _novedadService = novedadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _novedadService.ObtenerTodasAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var novedad = await _novedadService.ObtenerPorIdAsync(id);
            if (novedad == null)
                return NotFound(new { mensaje = $"No se encontró la novedad con ID {id}" });

            return Ok(novedad);
        }

        [HttpGet("obra/{idObra}")]
        public async Task<IActionResult> GetByObra(int idObra)
        {
            return Ok(await _novedadService.ObtenerPorObraAsync(idObra));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearNovedadDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _novedadService.CrearNovedadAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear la novedad." });

            return StatusCode(201, new { mensaje = "Novedad creada con éxito" });
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var resultado = await _novedadService.CambiarEstadoAsync(id, nuevoEstado);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró la novedad con ID {id}" });

            return Ok(new { mensaje = $"Estado actualizado a '{nuevoEstado}'" });
        }
    }
}