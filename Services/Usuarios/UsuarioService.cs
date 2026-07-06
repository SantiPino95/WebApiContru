using Microsoft.EntityFrameworkCore;
using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Usuarios;

namespace MiWebApi.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly MiWebApi.Data.DbcontruContext _context; // 👈 Agregar el contexto

        // 👈 Inyectar el contexto en el constructor
        public UsuarioService(IUsuarioRepository usuarioRepository, MiWebApi.Data.DbcontruContext context)
        {
            _usuarioRepository = usuarioRepository;
            _context = context;
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
                NombreRol = u.IdRolNavigation?.NombreRol ?? "Sin Rol",
                IdEmpleado = u.Empleado?.IdEmpleado,  // ✅ Agregar
                FechaReg = DateTime.Now

            }).ToList();
        }

        public async Task<bool> AprobarUsuarioAsync(int id, string rol, int? idEmpleado)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            // 1. Cambiar estado a Activo
            usuario.Estado = "Activo";

            // 2. Obtener el ID del rol por nombre
            var rolId = await ObtenerIdRolPorNombre(rol);
            if (rolId == 0) return false; // Rol no encontrado

            usuario.IdRol = rolId;

            // 3. Si se vincula a un empleado, actualizar el empleado
            if (idEmpleado.HasValue)
            {
                var empleado = await _context.Empleados.FindAsync(idEmpleado.Value);
                if (empleado != null)
                {
                    empleado.IdUsuario = usuario.IdUsuario;
                    _context.Empleados.Update(empleado);
                }
            }

            // 4. Guardar cambios
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        public async Task<bool> RechazarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            usuario.Estado = "Rechazado";
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        public async Task<bool> SuspenderUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            usuario.Estado = "Suspendido";
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        public async Task<bool> ReactivarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            usuario.Estado = "Activo";
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        public async Task<bool> CambiarRolAsync(int id, string nuevoRol)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            var rolId = await ObtenerIdRolPorNombre(nuevoRol);
            if (rolId == 0) return false;

            usuario.IdRol = rolId;
            return await _usuarioRepository.ActualizarAsync(usuario);
        }

        // ── MÉTODO AUXILIAR: Obtener ID del rol por nombre ──
        private async Task<int> ObtenerIdRolPorNombre(string nombreRol)
        {
            var rol = await _context.Rols
                .FirstOrDefaultAsync(r => r.NombreRol == nombreRol);

            return rol?.IdRol ?? 0;
        }
        public async Task<IEnumerable<UsuarioPendienteDto>> ObtenerTodosAsync()
        {
            var entidades = await _usuarioRepository.ListarTodosAsync();

            return entidades.Select(u => new UsuarioPendienteDto
            {
                IdUsuario = u.IdUsuario,
                Email = u.Email,
                NombreEmpleado = u.Empleado?.Nombre,
                ApellidoEmpleado = u.Empleado?.Apellido,
                Estado = u.Estado ?? "Pendiente",
                NombreRol = u.IdRolNavigation?.NombreRol ?? "Sin Rol",
                IdEmpleado = u.Empleado?.IdEmpleado,
                FechaReg = u.FechaReg ?? DateTime.Now
            }).ToList();
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            // Si el usuario está vinculado a un empleado, desvincularlo
            if (usuario.Empleado != null)
            {
                usuario.Empleado.IdUsuario = null;
                _context.Empleados.Update(usuario.Empleado);
            }

            _context.Usuarios.Remove(usuario);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}