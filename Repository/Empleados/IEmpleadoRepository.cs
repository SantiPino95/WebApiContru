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
        Task<IEnumerable<EmpleadoObra>> ListarEmpleadosAsignados();
        Task<EmpleadoObra?> AsignarEmpleadoAObraAsync(EmpleadoObra asignacion);
        Task<EmpleadoObra?> ObtenerAsignacionPorIdAsync(int idObra, int idEmpleado);
        Task<bool> ExisteEmpleadoEnCualquierObraAsync(int idEmpleado);
        Task<bool> EliminarAsignacionEmpleadoObraAsync(EmpleadoObra empleadoobra);

    }

}

