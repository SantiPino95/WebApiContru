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

        [HttpGet("obra/{idObra}")]
        public async Task<IActionResult> GetByObra(int idObra)
        {
            return Ok(await _seguimientoService.ObtenerPorObraAsync(idObra));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearSeguimientoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _seguimientoService.CrearSeguimientoAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el seguimiento." });

            return StatusCode(201, new { mensaje = "Seguimiento creado con éxito" });
        }
    }
}