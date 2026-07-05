using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Herramientas
{
    public class HerramientaRepository : IHerramientaRepository
    {
        private readonly DbcontruContext _context;

        public HerramientaRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Herramienta>> ListarTodasLasHerramientasAsync()
        {
            try
            {
                var result = await _context.Herramientas
                    .Include(h => h.HerramientasAsignada)
                        .ThenInclude(a => a.IdObraNavigation)
                    .ToListAsync();

                Console.WriteLine($"✅ Repository: {result.Count} herramientas encontradas");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR en ListarTodasLasHerramientasAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Herramienta?> ObtenerHerramientaPorIdAsync(int id)
        {
            return await _context.Herramientas
                .Include(h => h.HerramientasAsignada)
                    .ThenInclude(a => a.IdObraNavigation)
                .FirstOrDefaultAsync(h => h.IdHerramienta == id);
        }

        public async Task<bool> AgregarHerramientaAsync(Herramienta herramienta)
        {
            await _context.Herramientas.AddAsync(herramienta);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarHerramientaAsync(Herramienta herramienta)
        {
            _context.Herramientas.Update(herramienta);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarHerramientaAsync(Herramienta herramienta)
        {
            // ✅ Eliminar todas las asignaciones primero (por si hay)
            var asignaciones = _context.HerramientasAsignadas
                .Where(a => a.IdHerramienta == herramienta.IdHerramienta);

            if (await asignaciones.AnyAsync())
            {
                _context.HerramientasAsignadas.RemoveRange(asignaciones);
                await _context.SaveChangesAsync();
            }

            // Luego eliminar la herramienta
            _context.Herramientas.Remove(herramienta);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegistrarSalidaAsignacionAsync(HerramientasAsignada asignacion)
        {
            await _context.HerramientasAsignadas.AddAsync(asignacion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<HerramientasAsignada?> ObtenerAsignacionActivaAsync(int idHerramienta, int idObra)
        {
            return await _context.HerramientasAsignadas
                .FirstOrDefaultAsync(a => a.IdHerramienta == idHerramienta
                                       && a.IdObra == idObra
                                       && a.FechaDevolucion == null);
        }

        public async Task<bool> ActualizarAsignacionAsync(HerramientasAsignada asignacion)
        {
            _context.HerramientasAsignadas.Update(asignacion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}