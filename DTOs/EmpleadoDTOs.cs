    using System.ComponentModel.DataAnnotations;

    namespace MiWebApi.DTOs
    {
    // 1. DTO PARA REGISTRAR / EDITAR EMPLEADO
    public class CrearEmpleadoDTOs
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string Categoria { get; set; } = null!;

        
        public string? Telefono { get; set; } = null!;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(9,MinimumLength = 9 ,ErrorMessage = "La cédula no puede superar los 9 caracteres.")]
        public string Cedula { get; set; } = null!;

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; } = null!;

        public decimal ValorHora { get; set; }
       
    }

        // 2. DTO PARA MOSTRAR EN LOS LISTADOS (Para el Administrador)
        public class EmpleadoListadoDTOs
        {
            public int IdEmpleado { get; set; }
            public string NombreCompleto { get; set; } = null!;
            public string Categoria { get; set; } = null!;
            public string? Telefono { get; set; }
        public string Cedula { get; set; } = null!;
            public bool EstaAsignado { get; set; }
        public string Email { get; set; } = null!;
        public decimal ValorHora { get; set; } 

    }

    
    public class EmpleadoObraDTOs
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string RolEnObra { get; set; } = null!;
        public string? Telefono { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
    }


    // DTO para ASIGNAR empleado a una obra
    public class AsignarEmpleadoObraDto
    {
        public int IdEmpleado { get; set; }
        public int IdObra { get; set; }
        public string RolEnObra { get; set; } = string.Empty;
        public decimal ValorHoraAsignado { get; set; }
    }
}


