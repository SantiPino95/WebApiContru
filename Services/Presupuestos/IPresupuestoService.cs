using MiWebApi.DTOs;

namespace MiWebApi.Services.Presupuestos
{
    public interface IPresupuestoService
    {
        Task<IEnumerable<PresupuestoListadoDto>> ObtenerPresupuestosAsync();
        Task<bool> CrearPresupuestoAsync(CrearPresupuestoDto dto);
    }
}
