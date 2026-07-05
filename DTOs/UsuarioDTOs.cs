namespace MiWebApi.DTOs
{
    // ── DTO para listar usuarios pendientes ──
    public class UsuarioPendienteDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = null!;
        public string? NombreEmpleado { get; set; }
        public string? ApellidoEmpleado { get; set; }
        public string Estado { get; set; } = null!;
        public string NombreRol { get; set; } = null!;
    }

    // ── DTO para aprobar usuario ──
    public class AprobarUsuarioDto
    {
        public string Rol { get; set; } = null!;
        public int? IdEmpleado { get; set; }
    }

    // ── DTO para cambiar rol ──
    public class CambiarRolDto
    {
        public string Rol { get; set; } = null!;
    }
}