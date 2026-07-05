using MiWebApi.Entity;

namespace MiWebApi.Repository.Orden_Compra
{
   

     
        public interface IOrdenCompraRepository
        {
            Task<IEnumerable<OrdenCompra>> ListarTodasAsync();
            Task<OrdenCompra?> ObtenerPorIdAsync(int id);
            Task<bool> AgregarAsync(OrdenCompra orden);
            Task<bool> ActualizarAsync(OrdenCompra orden);
            Task<bool> EliminarAsync(OrdenCompra orden);
        }
    }

