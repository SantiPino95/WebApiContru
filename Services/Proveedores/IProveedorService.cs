using MiWebApi.DTOs;

namespace MiWebApi.Services.Proveedores
{
    public interface IProveedorService 
    {
        Task<IEnumerable<ProveedorListadoDto>> ObtenerTodosAsync();
        Task<bool> CrearProveedorAsync(CrearProveedorDto dto);
    }
}
