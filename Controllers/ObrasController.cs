using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Obras;

namespace MiWebApi.Controllers
{
    [ApiController] // Le dice a .NET que esta clase es una API
    [Route("api/[controller]")] // Define la URL automáticamente como: api/obras
    public class ObrasController : ControllerBase
    {
        private readonly IObraService _obraService;

        // Inyectamos el servicio
        public ObrasController(IObraService obraService)
        {
            _obraService = obraService;
        }

        // 1. GET: api/obras (Listar todas las obras para el Admin)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var obras = await _obraService.ObtenerObrasParaAdminAsync();
            return Ok(obras); // Devuelve Estado 200 OK con el JSON de las obras
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Al llamar a este método, 'obra' ahora guardará automáticamente un ObraDetalleDto
            var obra = await _obraService.ObtenerObraPorIdAsync(id);

            if (obra == null)
            {
                return NotFound(new { mensaje = $"No se encontró la obra con ID {id}" });
            }

            // Devuelve el objeto con los datos de la obra, el cliente y la lista de empleados
            return Ok(obra);
        }

        // 3. POST: api/obras (Crear una obra nueva)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearObraDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Error 400 si faltan datos obligatorios

            var resultado = await _obraService.CrearObraAsync(dto);
            if (!resultado)
            {
                return BadRequest(new { mensaje = "No se pudo crear la obra." });
            }

            return StatusCode(201, new { mensaje = "Obra creada con éxito" }); // 201 significa "Creado con éxito"
        }

        // 4. PUT: api/obras/5 (Editar una obra existente)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CrearObraDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _obraService.ActualizarObraAsync(id, dto);
            if (!resultado)
            {
                return NotFound(new { mensaje = $"No se pudo actualizar. No existe la obra con ID {id}" });
            }

            return Ok(new { mensaje = "Obra actualizada con éxito" });
        }

        // 5. DELETE: api/obras/5 (Borrado lógico de una obra)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _obraService.EliminarObraAsync(id);
            if (!resultado)
            {
                return NotFound(new { mensaje = $"No se pudo eliminar. No existe la obra con ID {id}" });
            }

            return Ok(new { mensaje = "Obra dada de baja con éxito" });
        }
    }
}