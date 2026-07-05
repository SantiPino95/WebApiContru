using MiWebApi.DTOs;

namespace MiWebApi.Services.Seguimientos
{
    public interface ISeguimientoService
    {
        Task<IEnumerable<SeguimientoListadoDto>> ObtenerPorObraAsync(int idObra);
        Task<SeguimientoListadoDto?> ObtenerUltimoPorObraAsync(int idObra);
        Task<bool> CrearSeguimientoAsync(CrearSeguimientoDto dto);
        Task<bool> EliminarSeguimientoAsync(int id);
    }
}