using MiWebApi.DTOs;

namespace MiWebApi.Services.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<CLientesListadoDTOs>> ObtenerClientesParaAdminAsync();
        Task<CLientesListadoDTOs?> ObtenerClientePorIdAsync(int id);
        Task<bool> CrearClienteAsync(CrearClienteDTOs dto);
        Task<bool> ActualizarClienteAsync(int id, CrearClienteDTOs dto);
        Task<bool> EliminarClienteAsync(int id);
    }
}
