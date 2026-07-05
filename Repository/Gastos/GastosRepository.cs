using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Gastos
{
    public class GastoRepository : IGastoRepository
    {
        private readonly DbcontruContext _context;

        public GastoRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GastosObra>> ListarTodosAsync()
        {
            return await _context.GastosObras
                .Include(g => g.IdObraNavigation)
                .ToListAsync();
        }

        public async Task<GastosObra?> ObtenerPorIdAsync(int id)
        {
            return await _context.GastosObras
                .Include(g => g.IdObraNavigation)
                .FirstOrDefaultAsync(g => g.IdGasto == id);
        }

        public async Task<IEnumerable<GastosObra>> ListarPorObraAsync(int idObra)
        {
            return await _context.GastosObras
                .Include(g => g.IdObraNavigation)
                .Where(g => g.IdObra == idObra)
                .ToListAsync();
        }

        public async Task<bool> AgregarAsync(GastosObra gasto)
        {
            await _context.GastosObras.AddAsync(gasto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(GastosObra gasto)
        {
            _context.GastosObras.Update(gasto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(GastosObra gasto)
        {
            _context.GastosObras.Remove(gasto);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}