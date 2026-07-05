using MiWebApi.DTOs;
using MiWebApi.Entity;


namespace MiWebApi.Services.Orden_Compra
{
    public interface IOrdenCompraService
    {
        Task<IEnumerable<OrdenCompraDTO>> ListarTodasAsync();

        Task<OrdenCompra?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearOrdenCompraDTO dto);

        Task<bool> ActualizarAsync(int idorden,OrdenCompraDTO dto);

        Task<bool> EliminarAsync(int id);

        Task<bool> ConfirmarEntregaAsync(int id);
    }
}