using MiWebApi.Entity;

namespace MiWebApi.Repository.Obras
{
    public interface IObraRepository
    {

        Task<IEnumerable<Obra>> ListarTodasLasObrasAsync();
        Task<Obra?> ObtenerObraPorIdAsync(int id);
        Task AgregarObraAsync(Obra obra);
        Task ActualizarObraAsync(Obra obra);
        Task EliminarObraAsync(Obra obra);
    }
}