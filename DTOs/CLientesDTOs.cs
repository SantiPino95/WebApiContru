using System.ComponentModel.DataAnnotations;

namespace MiWebApi.DTOs
{
    public class CLientesListadoDTOs
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public List<ObraResumenDto> Obras { get; set; } = new List<ObraResumenDto>();
    }

  
    public class ObraResumenDto
    {
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
    }


    public class CrearClienteDTOs
    {
        // El ID no lo necesitas aquí porque la base de datos lo genera solo (Identity)
        // Puedes borrarlo de este DTO de creación si quieres.

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!; // Obligatorio

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!; // Obligatorio

        // Si el teléfono y el email no son obligatorios para registrarse, usa '?'
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = null!;
    }


}
