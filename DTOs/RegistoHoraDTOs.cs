using System.ComponentModel.DataAnnotations;

namespace MiWebApi.DTOs
{
    // DTO de lectura — lo que se muestra en el historial / vista semanal
    public class RegistroHoraDto
    {
        public int IdRegistro { get; set; }
        public int IdEmpleadoObra { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }

    // DTO para CARGAR horas de hoy
    public class CrearRegistroHoraDto
    {
        [Required(ErrorMessage = "Debés indicar la asignación (obra).")]
        public int IdEmpleadoObra { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Range(0, 24, ErrorMessage = "Las horas comunes deben estar entre 0 y 24.")]
        public decimal HorasComunes { get; set; }

        [Range(0, 12, ErrorMessage = "Las horas extras deben estar entre 0 y 12.")]
        public decimal HorasExtras { get; set; }

        [MaxLength(255, ErrorMessage = "Las observaciones no pueden superar los 255 caracteres.")]
        public string? ObservacionesEmpleado { get; set; }
    }

    // DTO para EDITAR un registro existente (mismo día)
    public class ActualizarRegistroHoraDto
    {
        [Range(0, 24, ErrorMessage = "Las horas comunes deben estar entre 0 y 24.")]
        public decimal HorasComunes { get; set; }

        [Range(0, 12, ErrorMessage = "Las horas extras deben estar entre 0 y 12.")]
        public decimal HorasExtras { get; set; }

        [MaxLength(255, ErrorMessage = "Las observaciones no pueden superar los 255 caracteres.")]
        public string? ObservacionesEmpleado { get; set; }
    }
}