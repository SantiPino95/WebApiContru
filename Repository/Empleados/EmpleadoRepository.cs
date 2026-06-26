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
        }
    }


    

