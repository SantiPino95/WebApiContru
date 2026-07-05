using MiWebApi.DTOs;
using MiWebApi.Repository.Usuarios;

namespace MiWebApi.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<UsuarioPendienteDto>> ObtenerPendientesAsync()
        {
            var entidades = await _usuarioRepository.ListarPendientesAsync();

            return entidades.Select(u => new UsuarioPendienteDto
            {
                IdUsuario = u.IdUsuario,
                Email = u.Email,
                NombreEmpleado = u.Empleado?.Nombre,
                ApellidoEmpleado = u.Empleado?.Apellido,
                Estado = u.Estado ?? "Pendiente",
                NombreRol = u.IdRolNavigation?.NombreRol ?? "Sin Rol"
            }).ToList();
        }

        public async Task<bool> AprobarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            usuario.Estado = "Activo";
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        public async Task<bool> RechazarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            usuario.Estado = "Rechazado";
            return await _usuarioRepository.ActualizarAsync(usuario);
        }
    }
}