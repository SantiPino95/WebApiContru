using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.PagosProveedores
{
    public class PagoProveedorRepository : IPagoProveedorRepository
    {
        private readonly DbcontruContext _context;

        public PagoProveedorRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PagoProveedor>> ListarTodosAsync()
        {
            return await _context.PagoProveedors
                .Include(p => p.IdProveedorNavigation)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<PagoProveedor?> ObtenerPorIdAsync(int id)
        {
            return await _context.PagoProveedors
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(p => p.IdPagoProveedor == id);
        }

        public async Task<IEnumerable<PagoProveedor>> ListarPorProveedorAsync(int idProveedor)
        {
            return await _context.PagoProveedors
                .Include(p => p.IdProveedorNavigation)
                .Where(p => p.IdProveedor == idProveedor)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<bool> AgregarAsync(PagoProveedor pago)
        {
            await _context.PagoProveedors.AddAsync(pago);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(PagoProveedor pago)
        {
            _context.PagoProveedors.Update(pago);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(PagoProveedor pago)
        {
            _context.PagoProveedors.Remove(pago);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}