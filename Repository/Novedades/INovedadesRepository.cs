using MiWebApi.Entity;

namespace MiWebApi.Repository.Novedades
{
    public interface INovedadRepository
    {
        Task<IEnumerable<NovedadesObra>> ListarTodasAsync();
        Task<NovedadesObra?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<NovedadesObra>> ListarPorObraAsync(int idObra);
        Task<bool> AgregarAsync(NovedadesObra novedad);
        Task<bool> ActualizarAsync(NovedadesObra novedad);
        Task<bool> EliminarAsync(NovedadesObra novedad);
    }
}