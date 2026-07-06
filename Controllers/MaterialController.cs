using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiWebApi.Dtos;
using MiWebApi.Services.Materiales;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialesController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        // Constructor que inyecta el servicio de materiales
        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }


        // GET: api/Materiales
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _materialService.ObtenerMaterialesParaAdminAsync());
        }


        // GET: api/Materiales/{id}
        [HttpPost]
        public async Task<IActionResult> Create(CrearMaterialDto dto)
        {
            var resultado = await _materialService.AgregarMaterialAsync(dto);
            if (!resultado) return BadRequest("No se pudo crear el material.");
            return Ok("Material registrado con su stock inicial.");
        }


        // GET: api/Materiales/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CrearMaterialDto dto)
        {
            var resultado = await _materialService.ActualizarMaterialAsync(id, dto);
            if (!resultado) return NotFound("Material no encontrado.");
            return Ok("Material actualizado con éxito.");
        }


        // DELETE: api/Materiales/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _materialService.EliminarMaterialAsync(id);
            if (!resultado) return NotFound("Material no encontrado.");
            return Ok("Material eliminado del sistema.");
        }


        // POST: api/Materiales/consumir
        [HttpPost("consumir")]
        public async Task<IActionResult> ConsumirMaterial(int idMaterial, int idObra, decimal cantidad)
        {
            var resultado = await _materialService.RegistrarConsumoEnObraAsync(idMaterial, idObra, cantidad);
            if (!resultado) return BadRequest("No se pudo registrar el consumo. Verifica si el material existe o si hay stock disponible suficiente.");
            return Ok("Consumo registrado y stock actualizado correctamente.");
        }



        // POST: api/Materiales/entrada
        [HttpPost("entrada")]
        public async Task<IActionResult> ReponerStock([FromBody] ReponerStockDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _materialService.ReponerStockAsync(dto);
            if (!resultado)
                return NotFound(new { mensaje = $"No se encontró el material con ID {dto.IdMaterial}" });

            return Ok(new { mensaje = $"Stock repuesto con éxito. Se agregaron {dto.Cantidad} unidades." });
        }
    }
}
