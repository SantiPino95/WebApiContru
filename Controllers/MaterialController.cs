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

        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _materialService.ObtenerMaterialesParaAdminAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CrearMaterialDto dto)
        {
            var resultado = await _materialService.AgregarMaterialAsync(dto);
            if (!resultado) return BadRequest("No se pudo crear el material.");
            return Ok("Material registrado con su stock inicial.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CrearMaterialDto dto)
        {
            var resultado = await _materialService.ActualizarMaterialAsync(id, dto);
            if (!resultado) return NotFound("Material no encontrado.");
            return Ok("Material actualizado con éxito.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _materialService.EliminarMaterialAsync(id);
            if (!resultado) return NotFound("Material no encontrado.");
            return Ok("Material eliminado del sistema.");
        }

        [HttpPost("consumir")]
        public async Task<IActionResult> ConsumirMaterial(int idMaterial, int idObra, decimal cantidad)
        {
            var resultado = await _materialService.RegistrarConsumoEnObraAsync(idMaterial, idObra, cantidad);
            if (!resultado) return BadRequest("No se pudo registrar el consumo. Verifica si el material existe o si hay stock disponible suficiente.");
            return Ok("Consumo registrado y stock actualizado correctamente.");
        }
    }
}
