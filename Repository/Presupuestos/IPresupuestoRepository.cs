using MiWebApi.Entity;

namespace MiWebApi.Repository.Presupuestos
{
    public interface IPresupuestoRepository
    {
        Task<IEnumerable<Presupuesto>> ListarTodosConObrasAsync();
        Task<bool> AgregarPresupuestoCompletoAsync(Presupuesto presupuesto);
        Task<bool> CambiarEstadoAsync(int idPresupuesto, string nuevoEstado);
    }
}
