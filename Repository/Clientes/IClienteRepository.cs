using MiWebApi.Entity;  

namespace MiWebApi.Repository.Clientes
{
     public  interface  IClienteRepository
    {
        Task<IEnumerable<Cliente>> ListarTodosLosClientesAsync();
        Task<Cliente?> ObtenerClientePorIdAsync(int id);
        Task<bool> AgregarClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(Cliente cliente);
    }
}
