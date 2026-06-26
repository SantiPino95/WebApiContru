using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Clientes;

namespace MiWebApi.Controllers
{
        [ApiController] // Le dice a .NET que esta clase es una API
        [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _clienteService.ObtenerClientesParaAdminAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = $"No se encontró el cliente con ID {id}" });
            }
            return Ok(cliente);
        }


        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDTOs dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultado = await _clienteService.CrearClienteAsync(dto);
            if (!resultado)
            {
                return BadRequest(new { mensaje = "No se pudo crear el cliente." });
            }
            return StatusCode(201, new { mensaje = "Cliente creado con éxito" });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] CrearClienteDTOs dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultado = await _clienteService.ActualizarClienteAsync(id, dto);
            if (!resultado)
            {
                return NotFound(new { mensaje = "No se pudo actualizar el cliente." });
            }
            return Ok(new { mensaje = "Cliente actualizado con éxito" });
        }


        [HttpDelete("{id}")] 
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var resultado = await _clienteService.EliminarClienteAsync(id);
            if (!resultado)
            {
                return BadRequest(new { mensaje = "No se pudo eliminar el cliente." });
            }
            return Ok(new { mensaje = "Cliente eliminado con éxito" });
        }
    }
}
