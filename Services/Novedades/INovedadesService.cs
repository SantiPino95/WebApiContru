using MiWebApi.DTOs;

namespace MiWebApi.Services.Novedades
{
    public interface INovedadService
    {
        Task<IEnumerable<NovedadListadoDto>> ObtenerTodasAsync();
        Task<NovedadListadoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<NovedadListadoDto>> ObtenerPorObraAsync(int idObra);
        Task<bool> CrearNovedadAsync(CrearNovedadDto dto);
        Task<bool> CambiarEstadoAsync(int id, string nuevoEstado);
        Task<bool> EliminarNovedadAsync(int id);
    }
}