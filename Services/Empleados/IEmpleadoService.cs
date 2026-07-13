using MiWebApi.DTOs;

namespace MiWebApi.Services.Empleados
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoListadoDTOs>> ObtenerEmpleadosParaAdminAsync();
        Task<IEnumerable<EmpleadoObraDTOs>> ObtenerEmpleadosAsignadosAsync();
        Task<bool> AsignarEmpleadoAObraAsync(AsignarEmpleadoObraDto dto);
        Task<EmpleadoListadoDTOs?> ObtenerEmpleadoPorIdAsync(int id);
        Task<EmpleadoObraDTOs?> ObtenerAsignacionPorIdAsync(int idObra, int idEmpleado);
        Task<bool> CrearEmpleadoAsync(CrearEmpleadoDTOs dto);
        Task<bool> ActualizarEmpleadoAsync(int id, CrearEmpleadoDTOs dto);
        Task<bool> EliminarEmpleadoAsync(int id);
        Task<bool> EliminarAsignacionEmpleadoObraAsync(int idObra, int idEmpleado);
    }
}
