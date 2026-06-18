using Microsoft.AspNetCore.Mvc;
using MiWebApi.Services;
using MiWebApi.Models;

namespace MiWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObraController : ControllerBase
    {
        private readonly IObraService _service;

        public ObraController(IObraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obra>>> GetAll()
        {
            var obras = await _service.GetAllAsync();
            return Ok(obras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Obra>> Get(int id)
        {
            var obra = await _service.GetByIdAsync(id);
            if (obra == null) return NotFound();
            return Ok(obra);
        }

        [HttpPost]
        public async Task<ActionResult<Obra>> Create([FromBody] Obra obra)
        {
            var created = await _service.CreateAsync(obra);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Obra>> Update(int id, [FromBody] Obra obra)
        {
            var updated = await _service.UpdateAsync(id, obra);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
