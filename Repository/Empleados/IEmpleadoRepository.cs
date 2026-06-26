using MiWebApi.Entity;

namespace MiWebApi.Repository.Empleados
{
    public interface IEmpleadoRepository
    {
       
       
            Task<IEnumerable<Empleado>> ListarTodosLosEmpleadosAsync();
            Task<Empleado?> ObtenerEmpleadoPorIdAsync(int id);
            Task AgregarEmpleadoAsync(Empleado empleado);
            Task ActualizarEmpleadoAsync(Empleado empleado);
            Task EliminarEmpleadoAsync(Empleado empleado);
        
    }

}

