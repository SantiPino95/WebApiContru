using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.PagosProveedores;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoProveedorController : ControllerBase
    {
        private readonly IPagoProveedorService _pagoService;

        public PagoProveedorController(IPagoProveedorService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pagoService.ObtenerTodosAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pago = await _pagoService.ObtenerPorIdAsync(id);
            if (pago == null)
                return NotFound(new { mensaje = $"No se encontró el pago con ID {id}" });

            return Ok(pago);
        }

        [HttpGet("proveedor/{idProveedor}")]
        public async Task<IActionResult> GetByProveedor(int idProveedor)
        {
            return Ok(await _pagoService.ObtenerPorProveedorAsync(idProveedor));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearPagoProveedorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _pagoService.CrearPagoAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo registrar el pago." });

            return StatusCode(201, new { mensaje = "Pago registrado con éxito" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _pagoService.EliminarPagoAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el pago con ID {id}" });

            return Ok(new { mensaje = "Pago eliminado con éxito" });
        }
    }
}