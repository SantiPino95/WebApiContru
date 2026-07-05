using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Proveedores;

namespace MiWebApi.Controllers
{
    namespace MiWebApi.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ProveedoresController : ControllerBase
        {
            private readonly IProveedorService _service;
            public ProveedoresController(IProveedorService service) => _service = service;

            [HttpGet]
            public async Task<IActionResult> Get() => Ok(await _service.ObtenerTodosAsync());

            [HttpPost]
            public async Task<IActionResult> Post(CrearProveedorDto dto)
            {
                var res = await _service.CrearProveedorAsync(dto);
                return res ? Ok("Proveedor registrado.") : BadRequest("El RUT ya existe.");
            }





            // PUT: api/Proveedores/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] CrearProveedorDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.ActualizarProveedorAsync(id, dto);
                if (!resultado)
                    return NotFound(new { mensaje = $"No se encontró el proveedor con ID {id} o el RUT ya existe." });

                return Ok(new { mensaje = "Proveedor actualizado con éxito" });
            }
        }


    }
}
