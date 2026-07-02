using MiWebApi.Data;
using MiWebApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace MiWebApi.Repository.Presupuestos
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly DbcontruContext _context;
        public PresupuestoRepository(DbcontruContext context) => _context = context;

        public async Task<IEnumerable<Presupuesto>> ListarTodosConObrasAsync()
        {
            return await _context.Presupuestos
                .Include(p => p.IdObraNavigation)
                .Include(p => p.DetallePresupuestos)
                .ToListAsync();
        }

        public async Task<bool> AgregarPresupuestoCompletoAsync(Presupuesto presupuesto)
        {
            await _context.Presupuestos.AddAsync(presupuesto);
            return await _context.SaveChangesAsync() > 0; // Guarda cabecera y detalles en combo
        }

        public async Task<bool> CambiarEstadoAsync(int idPresupuesto, string nuevoEstado)
        {
            var pres = await _context.Presupuestos.FindAsync(idPresupuesto);
            if (pres == null) return false;

            pres.EstadoPresupuesto = nuevoEstado;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
