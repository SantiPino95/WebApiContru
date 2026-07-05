using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Novedades
{
    public class NovedadRepository : INovedadRepository
    {
        private readonly DbcontruContext _context;

        public NovedadRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NovedadesObra>> ListarTodasAsync()
        {
            return await _context.NovedadesObras
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdEmpleadoNavigation)
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdObraNavigation)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<NovedadesObra?> ObtenerPorIdAsync(int id)
        {
            return await _context.NovedadesObras
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdEmpleadoNavigation)
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdObraNavigation)
                .FirstOrDefaultAsync(n => n.IdNovedad == id);
        }

        public async Task<IEnumerable<NovedadesObra>> ListarPorObraAsync(int idObra)
        {
            return await _context.NovedadesObras
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdEmpleadoNavigation)
                .Include(n => n.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdObraNavigation)
                .Where(n => n.IdEmpleadoObraNavigation.IdObra == idObra)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<bool> AgregarAsync(NovedadesObra novedad)
        {
            await _context.NovedadesObras.AddAsync(novedad);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(NovedadesObra novedad)
        {
            _context.NovedadesObras.Update(novedad);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(NovedadesObra novedad)
        {
            _context.NovedadesObras.Remove(novedad);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}