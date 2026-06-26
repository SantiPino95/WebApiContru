using MiWebApi.DTOs;

namespace MiWebApi.Services.Obras
{
    public interface IObraService
    {
        Task<IEnumerable<ObraAdminListadoDto>> ObtenerObrasParaAdminAsync();
        Task<ObraDetalleDto?> ObtenerObraPorIdAsync(int id);
        Task<bool> CrearObraAsync(CrearObraDto dto);
        Task<bool> ActualizarObraAsync(int id, CrearObraDto dto);
        Task<bool> EliminarObraAsync(int id);
        
    }
}
