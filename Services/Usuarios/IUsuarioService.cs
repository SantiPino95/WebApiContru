using MiWebApi.DTOs;

namespace MiWebApi.Services.Usuarios
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioPendienteDto>> ObtenerPendientesAsync();
        Task<bool> AprobarUsuarioAsync(int id);
        Task<bool> RechazarUsuarioAsync(int id);
    }
}