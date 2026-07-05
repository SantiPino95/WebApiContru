using MiWebApi.DTOs;

namespace MiWebApi.Services.PagosProveedores
{
    public interface IPagoProveedorService
    {
        Task<IEnumerable<PagoProveedorListadoDto>> ObtenerTodosAsync();
        Task<PagoProveedorListadoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<PagoProveedorListadoDto>> ObtenerPorProveedorAsync(int idProveedor);
        Task<bool> CrearPagoAsync(CrearPagoProveedorDto dto);
        Task<bool> EliminarPagoAsync(int id);
    }
}