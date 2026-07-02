using Microsoft.AspNetCore.Mvc;
using MiWebApi.Services.Presupuestos;

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
    }
}
}
