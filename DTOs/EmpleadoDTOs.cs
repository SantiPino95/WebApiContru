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

            public string? Telefono { get; set; }
        }

        // 2. DTO PARA MOSTRAR EN LOS LISTADOS (Para el Administrador)
        public class EmpleadoListadoDTOs
        {
            public int IdEmpleado { get; set; }
            public string NombreCompleto { get; set; } = null!;
            public string Categoria { get; set; } = null!;
            public string? Telefono { get; set; }
        }

    

    
}


