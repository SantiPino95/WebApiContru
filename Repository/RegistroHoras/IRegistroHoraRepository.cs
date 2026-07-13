using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.RegistroHoras
{
    public interface IRegistroHoraRepository
    {
        Task<List<RegistroHora>> ListarPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes);
        Task<RegistroHora?> ObtenerPorIdAsync(int idRegistro);
        Task<RegistroHora?> ObtenerPorEmpleadoObraYFechaAsync(int idEmpleadoObra, DateTime fecha);
        Task<EmpleadoObra?> ObtenerEmpleadoObraAsync(int idEmpleadoObra);
        Task AgregarAsync(RegistroHora registro);
        Task ActualizarAsync(RegistroHora registro);
    }

    public class RegistroHoraRepository : IRegistroHoraRepository
    {
        private readonly DbcontruContext _context;

        public RegistroHoraRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<List<RegistroHora>> ListarPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes)
        {
            return await _context.RegistroHoras
                .Include(r => r.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdObraNavigation)
                .Where(r => r.IdEmpleadoObraNavigation.IdEmpleado == idEmpleado
                            && r.Fecha.Year == anio
                            && r.Fecha.Month == mes)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }

        public async Task<RegistroHora?> ObtenerPorIdAsync(int idRegistro)
        {
            return await _context.RegistroHoras
                .Include(r => r.IdEmpleadoObraNavigation)
                    .ThenInclude(eo => eo.IdObraNavigation)
                .FirstOrDefaultAsync(r => r.IdRegistro == idRegistro);
        }

        public async Task<RegistroHora?> ObtenerPorEmpleadoObraYFechaAsync(int idEmpleadoObra, DateTime fecha)
        {
            return await _context.RegistroHoras
                .FirstOrDefaultAsync(r => r.IdEmpleadoObra == idEmpleadoObra && r.Fecha.Date == fecha.Date);
        }

        public async Task<EmpleadoObra?> ObtenerEmpleadoObraAsync(int idEmpleadoObra)
        {
            return await _context.EmpleadoObras
                .Include(eo => eo.IdObraNavigation)
                .FirstOrDefaultAsync(eo => eo.IdEmpleadoObra == idEmpleadoObra);
        }

        public async Task AgregarAsync(RegistroHora registro)
        {
            _context.RegistroHoras.Add(registro);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(RegistroHora registro)
        {
            _context.RegistroHoras.Update(registro);
            await _context.SaveChangesAsync();
        }
    }
}