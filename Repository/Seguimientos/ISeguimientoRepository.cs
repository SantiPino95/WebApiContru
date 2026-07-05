using MiWebApi.Entity;

namespace MiWebApi.Repository.Seguimientos
{
    public interface ISeguimientoRepository
    {
        Task<IEnumerable<SeguimientoObra>> ListarPorObraAsync(int idObra);
        Task<SeguimientoObra?> ObtenerUltimoPorObraAsync(int idObra);
        Task<bool> AgregarAsync(SeguimientoObra seguimiento);
        Task<bool> ActualizarAsync(SeguimientoObra seguimiento);
        Task<bool> EliminarAsync(SeguimientoObra seguimiento);
    }
}