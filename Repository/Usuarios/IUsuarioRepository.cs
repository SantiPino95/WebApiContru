using MiWebApi.Entity;

namespace MiWebApi.Repository.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ListarPendientesAsync();
        Task<IEnumerable<Usuario>> ListarTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<bool> ActualizarAsync(Usuario usuario);
    }
}