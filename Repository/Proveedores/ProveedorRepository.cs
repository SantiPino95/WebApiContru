using MiWebApi.Data;
using MiWebApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace MiWebApi.Repository.Proveedores
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly DbcontruContext _context;
        public ProveedorRepository(DbcontruContext context) => _context = context;

        public async Task<IEnumerable<Proveedor>> ListarTodosAsync() => await _context.Proveedors.ToListAsync();
        public async Task<Proveedor?> ObtenerPorIdAsync(int id) => await _context.Proveedors.FindAsync(id);
        public async Task<Proveedor?> ObtenerPorRutAsync(string rut) => await _context.Proveedors.FirstOrDefaultAsync(p => p.Rut == rut);

        public async Task<bool> AgregarAsync(Proveedor proveedor)
        {
            await _context.Proveedors.AddAsync(proveedor);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ActualizarAsync(Proveedor proveedor)
        {
            _context.Proveedors.Update(proveedor);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> EliminarAsync(Proveedor proveedor)
        {
            _context.Proveedors.Remove(proveedor);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

