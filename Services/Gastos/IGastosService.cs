using MiWebApi.DTOs;

namespace MiWebApi.Services.Gastos
{
    public interface IGastoService
    {
        Task<IEnumerable<GastoListadoDto>> ObtenerTodosAsync();
        Task<GastoListadoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<GastoListadoDto>> ObtenerPorObraAsync(int idObra);
        Task<bool> CrearGastoAsync(CrearGastoDto dto);
        Task<bool> ActualizarGastoAsync(int id, ActualizarGastoDto dto);
        Task<bool> EliminarGastoAsync(int id);
    }
}