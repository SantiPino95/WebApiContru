using MiWebApi.DTOs;

namespace MiWebApi.Services.Presupuestos
{
    public interface IPresupuestoService
    {
        Task<IEnumerable<PresupuestoListadoDto>> ObtenerPresupuestosAsync();
        Task<bool> CrearPresupuestoAsync(CrearPresupuestoDto dto);
        Task<bool> CambiarEstadoAsync(int idPresupuesto, string nuevoEstado); // NUEVO
    }
}
