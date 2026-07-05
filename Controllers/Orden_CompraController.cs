using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Orden_Compra;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _ordenCompraService;

        public OrdenCompraController(IOrdenCompraService ordenCompraService)
        {
            _ordenCompraService = ordenCompraService;
        }

        // GET: api/OrdenCompra
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordenes = await _ordenCompraService.ListarTodasAsync();
            return Ok(ordenes);
        }

        // GET: api/OrdenCompra/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orden = await _ordenCompraService.ObtenerPorIdAsync(id);
            if (orden == null)
                return NotFound(new { mensaje = $"No se encontró la orden con ID {id}" });

            return Ok(orden);
        }

        // POST: api/OrdenCompra
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearOrdenCompraDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _ordenCompraService.CrearAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear la orden de compra." });

            return StatusCode(201, new { mensaje = "Orden de compra creada con éxito" });
        }

        // PUT: api/OrdenCompra/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrdenCompraDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _ordenCompraService.ActualizarAsync(id, dto);
            if (!resultado)
                return NotFound(new { mensaje = $"No se pudo actualizar la orden {id}" });

            return Ok(new { mensaje = "Orden actualizada con éxito" });
        }

        // DELETE: api/OrdenCompra/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _ordenCompraService.EliminarAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró la orden con ID {id}" });

            return Ok(new { mensaje = "Orden eliminada con éxito" });
        }

        // PATCH: api/OrdenCompra/{id}/confirmar-entrega
        [HttpPatch("{id}/confirmar-entrega")]
        public async Task<IActionResult> ConfirmarEntrega(int id)
        {
            var resultado = await _ordenCompraService.ConfirmarEntregaAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = $"No se pudo confirmar la entrega. Orden {id} no encontrada o no está pendiente." });

            return Ok(new { mensaje = "Entrega confirmada y stock actualizado con éxito" });
        }
    }
}