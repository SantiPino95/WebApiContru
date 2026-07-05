using System.ComponentModel.DataAnnotations;

namespace MiWebApi.DTOs
{
    // DTO para REGISTRO de nuevo usuario
    public class RegisterDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        public string Cedula { get; set; } = null!;

        public string? Telefono { get; set; }

        public decimal ValorHora { get; set; } = 0;

        public string Categoria { get; set; } = "Oficial";

        public int IdRol { get; set; } = 2; // Por defecto "Empleado" (rol básico)
    }

    // DTO para LOGIN
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = null!;
    }

    // DTO para RESPUESTA de Login (devuelve el token)
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public int IdUsuario { get; set; }
        public DateTime Expira { get; set; }
    }
}