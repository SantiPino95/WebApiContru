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
        }
    }
}
