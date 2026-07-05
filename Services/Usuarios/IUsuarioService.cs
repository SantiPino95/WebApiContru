using MiWebApi.DTOs;

namespace MiWebApi.Services.Usuarios
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioPendienteDto>> ObtenerPendientesAsync();
        Task<bool> AprobarUsuarioAsync(int id, string rol, int? idEmpleado);
        Task<bool> RechazarUsuarioAsync(int id);
        Task<bool> SuspenderUsuarioAsync(int id);
        Task<bool> ReactivarUsuarioAsync(int id);
        Task<bool> CambiarRolAsync(int id, string nuevoRol);
    }
}