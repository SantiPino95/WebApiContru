using Microsoft.EntityFrameworkCore;
using MiWebApi.DTOs;
using MiWebApi.Data;
using MiWebApi.Repository.Usuarios;
using MiWebApi.Repository.Empleados;

namespace MiWebApi.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly DbcontruContext _context; 

        
        public UsuarioService(IUsuarioRepository usuarioRepository, IEmpleadoRepository empleadoRepository, DbcontruContext context)
        {
            _usuarioRepository = usuarioRepository;
            _empleadoRepository = empleadoRepository;
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
                IdEmpleado = u.Empleado?.IdEmpleado,  
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

        public async Task<UsuarioPendienteDto?> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return null;
            return new UsuarioPendienteDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                NombreEmpleado = usuario.Empleado?.Nombre,
                ApellidoEmpleado = usuario.Empleado?.Apellido,
                Estado = usuario.Estado ?? "Pendiente",
                NombreRol = usuario.IdRolNavigation?.NombreRol ?? "Sin Rol",
                IdEmpleado = usuario.Empleado?.IdEmpleado,
                FechaReg = usuario.FechaReg ?? DateTime.Now
            };
        }
        public async Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto)
        {
            // 1. Buscar el usuario
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) return false;

            // 2. Actualizar datos de usuario
            usuario.Email = dto.Email;
            // Si manejas Rol por string o IdRol, ajusta aquí:
            // usuario.Rol = dto.Rol; 

            await _usuarioRepository.ActualizarAsync(usuario);

            // 3. GESTIÓN DE VINCULACIÓN CON EMPLEADO

            // A) Buscar si este usuario YA tenía un empleado asignado previamente
            var empleadoActual = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(id);

            // B) Si el empleado cambió o si seleccionó "Sin empleado" (null)
            if (empleadoActual != null && empleadoActual.IdEmpleado != dto.IdEmpleado)
            {
                empleadoActual.IdUsuario = null; // Liberamos el vínculo anterior
                await _empleadoRepository.ActualizarEmpleadoAsync(empleadoActual);
            }

            // C) Si seleccionó un nuevo empleado en el modal
            if (dto.IdEmpleado.HasValue && dto.IdEmpleado.Value > 0)
            {
                // Si es un empleado distinto al que ya tenía
                if (empleadoActual == null || empleadoActual.IdEmpleado != dto.IdEmpleado.Value)
                {
                    var nuevoEmpleado = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(dto.IdEmpleado.Value);
                    if (nuevoEmpleado != null)
                    {
                        nuevoEmpleado.IdUsuario = usuario.IdUsuario; // Asignamos la FK
                        await _empleadoRepository.ActualizarEmpleadoAsync(nuevoEmpleado);
                    }
                }
            }

            return true;
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