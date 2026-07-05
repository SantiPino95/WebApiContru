using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Seguimientos
{
    public class SeguimientoRepository : ISeguimientoRepository
    {
        private readonly DbcontruContext _context;

        public SeguimientoRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeguimientoObra>> ListarPorObraAsync(int idObra)
        {
            return await _context.SeguimientoObras
                .Include(s => s.IdObraNavigation)
                .Where(s => s.IdObra == idObra)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
        }

        public async Task<SeguimientoObra?> ObtenerUltimoPorObraAsync(int idObra)
        {
            return await _context.SeguimientoObras
                .Include(s => s.IdObraNavigation)
                .Where(s => s.IdObra == idObra)
                .OrderByDescending(s => s.Fecha)
                .ThenByDescending(s => s.IdSeguimiento)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AgregarAsync(SeguimientoObra seguimiento)
        {
            await _context.SeguimientoObras.AddAsync(seguimiento);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(SeguimientoObra seguimiento)
        {
            _context.SeguimientoObras.Update(seguimiento);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(SeguimientoObra seguimiento)
        {
            _context.SeguimientoObras.Remove(seguimiento);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}