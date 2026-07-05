using MiWebApi.Entity;

namespace MiWebApi.Repository.Gastos
{
    public interface IGastoRepository
    {
        Task<IEnumerable<GastosObra>> ListarTodosAsync();
        Task<GastosObra?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<GastosObra>> ListarPorObraAsync(int idObra);
        Task<bool> AgregarAsync(GastosObra gasto);
        Task<bool> ActualizarAsync(GastosObra gasto);
        Task<bool> EliminarAsync(GastosObra gasto);
    }
}