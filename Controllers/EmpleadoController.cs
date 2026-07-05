using Microsoft.AspNetCore.Mvc;
using MiWebApi.DTOs;
using MiWebApi.Services.Empleados;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL será: api/Empleado
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadoController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        // 1. LISTAR TODOS (GET: api/Empleado)
        [HttpGet]
        public async Task<IActionResult> ListarEmpleados()
        {
            var empleados = await _empleadoService.ObtenerEmpleadosParaAdminAsync();
            return Ok(empleados);
        }

        // 2. OBTENER POR ID (GET: api/Empleado/5)
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEmpleadoPorId(int id)
        {
            var empleado = await _empleadoService.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
            {
                return NotFound(new { mensaje = $"No se encontró el empleado con ID {id}" });
            }
            return Ok(empleado);
        }

        // 3. CREAR (POST: api/Empleado)
        [HttpPost]
        public async Task<IActionResult> CrearEmpleado([FromBody] CrearEmpleadoDTOs dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _empleadoService.CrearEmpleadoAsync(dto);
            return StatusCode(201, new { mensaje = "Empleado creado con éxito" });
        }

        // 4. ACTUALIZAR (PUT: api/Empleado/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] CrearEmpleadoDTOs dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _empleadoService.ActualizarEmpleadoAsync(id, dto);
            if (!resultado)
            {
                return NotFound(new { mensaje = $"No se pudo encontrar o actualizar el empleado con ID {id}." });
            }
            return Ok(new { mensaje = "Empleado actualizado con éxito" });
        }

        // 5. ELIMINAR (DELETE: api/Empleado/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var resultado = await _empleadoService.EliminarEmpleadoAsync(id);
            if (!resultado)
            {
                return NotFound(new { mensaje = $"No se pudo encontrar o eliminar el empleado con ID {id}." });
            }
            return Ok(new { mensaje = "Empleado eliminado con éxito" });
        }
        // 6. OBTENER EMPLEADOS ASIGNADOS (GET: api/Empleado/asignados)
        [HttpGet("asignados")]
        public async Task<IActionResult> ObtenerEmpleadosAsignados()
        {
            var empleados = await _empleadoService.ObtenerEmpleadosAsignadosAsync();
            return Ok(empleados);
        }

        // POST: api/Empleado/asignar
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarAObra([FromBody] AsignarEmpleadoObraDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _empleadoService.AsignarEmpleadoAObraAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo asignar el empleado a la obra." });

            return StatusCode(201, new { mensaje = "Empleado asignado a la obra con éxito" });
        }
    }
}
