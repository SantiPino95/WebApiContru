using MiWebApi.Entity;

namespace MiWebApi.Repository.PagosProveedores
{
    public interface IPagoProveedorRepository
    {
        Task<IEnumerable<PagoProveedor>> ListarTodosAsync();
        Task<PagoProveedor?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<PagoProveedor>> ListarPorProveedorAsync(int idProveedor);
        Task<bool> AgregarAsync(PagoProveedor pago);
        Task<bool> ActualizarAsync(PagoProveedor pago);
        Task<bool> EliminarAsync(PagoProveedor pago);
    }
}