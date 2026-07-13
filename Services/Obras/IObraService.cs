using MiWebApi.DTOs;

namespace MiWebApi.Services.Obras
{
    public interface IObraService
    {
        Task<IEnumerable<ObraAdminListadoDto>> ObtenerObrasParaAdminAsync();
        Task<ObraDetalleDto?> ObtenerObraPorIdAsync(int id);
        Task<bool> ExisteObraPorCodigoPublicoAsync(string codigoPublico);
        Task<bool> CrearObraAsync(CrearObraDto dto);
        Task<bool> ActualizarObraAsync(int id, ObraAdminListadoDto dto);
        Task<bool> EliminarObraAsync(int id);
        
    }
}
