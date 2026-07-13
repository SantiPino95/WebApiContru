using MiWebApi.Data;
using MiWebApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace MiWebApi.Repository.Empleados
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly DbcontruContext _context;

        public EmpleadoRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> ListarTodosLosEmpleadosAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public async Task<Empleado?> ObtenerEmpleadoPorIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task AgregarEmpleadoAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEmpleadoAsync(Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarEmpleadoAsync(Empleado empleado)
        {
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmpleadoObra>> ListarEmpleadosAsignados()
        {
            return await _context.EmpleadoObras.Include(eo => eo.IdEmpleadoNavigation)
                                               .Include(eo => eo.IdObraNavigation)
                                               .ToListAsync();


        }

        public async Task<EmpleadoObra?> AsignarEmpleadoAObraAsync(EmpleadoObra asignacion)
        {
            _context.EmpleadoObras.Add(asignacion);
            await _context.SaveChangesAsync();
            return asignacion;
        }

        public async Task<EmpleadoObra?> ObtenerAsignacionPorIdAsync(int idObra, int idEmpleado)
        {
            return await _context.EmpleadoObras
                                 .FirstOrDefaultAsync(eo => eo.IdObra == idObra && eo.IdEmpleado == idEmpleado);
        }

        public async Task<bool> ExisteEmpleadoEnCualquierObraAsync(int idEmpleado)
        {
            // Revisa si existe AL MENOS UNA asignación para este empleado en la tabla
            return await _context.EmpleadoObras
                                 .AnyAsync(eo => eo.IdEmpleado == idEmpleado);
        }


        public async Task<bool> EliminarAsignacionEmpleadoObraAsync(EmpleadoObra empleadoobra)
        {
            var asignacion = await ObtenerAsignacionPorIdAsync(empleadoobra.IdObra, empleadoobra.IdEmpleado );
            if (asignacion == null)
            {
                return false;
            }
            _context.EmpleadoObras.Remove(asignacion);
          return  await _context.SaveChangesAsync() > 0;
            
        }
    }
}


    

