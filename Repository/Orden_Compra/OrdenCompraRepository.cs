using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;


namespace MiWebApi.Repository.Orden_Compra
{
    public class OrdenCompraRepository : IOrdenCompraRepository
    {
        private readonly DbcontruContext _context;

        public OrdenCompraRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenCompra>> ListarTodasAsync()
        {
            return await _context.OrdenCompras
                .Include(o => o.IdProveedorNavigation)
                .Include(o => o.DetalleOrdenCompras)
                    .ThenInclude(d => d.IdMaterialNavigation)
                .ToListAsync();
        }

        public async Task<OrdenCompra?> ObtenerPorIdAsync(int id)
        {
            return await _context.OrdenCompras
                .Include(o => o.IdProveedorNavigation)
                .Include(o => o.DetalleOrdenCompras)
                    .ThenInclude(d => d.IdMaterialNavigation)
                .FirstOrDefaultAsync(o => o.IdOrden == id);
        }

        public async Task<bool> AgregarAsync(OrdenCompra orden)
        {
            _context.OrdenCompras.Add(orden);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(OrdenCompra orden)
        {
            _context.OrdenCompras.Update(orden);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(OrdenCompra orden)
        {
            _context.OrdenCompras.Remove(orden);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}







 