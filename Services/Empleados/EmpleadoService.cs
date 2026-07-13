using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Empleados;

namespace MiWebApi.Services.Empleados
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        

        public EmpleadoService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<EmpleadoListadoDTOs>> ObtenerEmpleadosParaAdminAsync()
        {
            // 1. Traemos todos los empleados
            var listaEntidades = await _empleadoRepository.ListarTodosLosEmpleadosAsync();

            
            // 2. Traemos todas las asignaciones activas (necesitas tener este repositorio inyectado)
            var listaAsignaciones = await _empleadoRepository.ListarEmpleadosAsignados();
            // 3. Armamos los DTOs con la lógica ya resuelta
            return listaEntidades.Select(e => new EmpleadoListadoDTOs
            {
                IdEmpleado = e.IdEmpleado,
                NombreCompleto = $"{e.Nombre} {e.Apellido}",
                Categoria = e.Categoria,
                Telefono = e.Telefono,
                Cedula = e.Cedula,
                Email = e.Email ?? string.Empty,
                ValorHora = e.ValorHora,

                 
                // Buscamos si el ID del empleado existe en la lista de asignaciones
                EstaAsignado = listaAsignaciones.Any(a => a.IdEmpleado == e.IdEmpleado),

               

            }).ToList();
        }




        public async Task<IEnumerable<EmpleadoObraDTOs>> ObtenerEmpleadosAsignadosAsync()
        {

            var listaEntidades = await _empleadoRepository.ListarEmpleadosAsignados();
            return listaEntidades.Select(e => new EmpleadoObraDTOs
            {
                IdEmpleadoObra = e.IdEmpleadoObra,   
                IdEmpleado = e.IdEmpleado,
                NombreCompleto = $"{e.IdEmpleadoNavigation.Nombre} {e.IdEmpleadoNavigation.Apellido}",
                Categoria = e.IdEmpleadoNavigation.Categoria,
                RolEnObra = e.RolEnObra,
                Telefono = e.IdEmpleadoNavigation.Telefono,
                IdObra = e.IdObra,
                NombreObra = e.IdObraNavigation.NombreObra
            }).ToList();
        }

        public async Task<EmpleadoListadoDTOs?> ObtenerEmpleadoPorIdAsync(int id)
        {
            var empleado = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null) return null;

            return new EmpleadoListadoDTOs
            {
                IdEmpleado = empleado.IdEmpleado,
                NombreCompleto = $"{empleado.Nombre} {empleado.Apellido}",
                Categoria = empleado.Categoria,
                Telefono = empleado.Telefono
            };
        }


      public async Task<EmpleadoObraDTOs?> ObtenerAsignacionPorIdAsync(int idObra, int idEmpleado)
        {
            var asignacion = await _empleadoRepository.ObtenerAsignacionPorIdAsync(idObra, idEmpleado);
            if (asignacion == null) return null;
            return new EmpleadoObraDTOs
            {
                IdEmpleadoObra = asignacion.IdEmpleadoObra,
                IdEmpleado = asignacion.IdEmpleado,
                NombreCompleto = $"{asignacion.IdEmpleadoNavigation.Nombre} {asignacion.IdEmpleadoNavigation.Apellido}",
                Categoria = asignacion.IdEmpleadoNavigation.Categoria,
                Telefono = asignacion.IdEmpleadoNavigation.Telefono,
                IdObra = asignacion.IdObra,
                NombreObra = asignacion.IdObraNavigation.NombreObra
            };
        }



        public async Task<bool> CrearEmpleadoAsync(CrearEmpleadoDTOs dto)
        {
            var nuevoEmpleado = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Categoria = dto.Categoria,
                Telefono = dto.Telefono,
                Cedula = dto.Cedula,
                Email = dto.Email,
                ValorHora = dto.ValorHora
            };

            await _empleadoRepository.AgregarEmpleadoAsync(nuevoEmpleado);
            return true;
        }

        public async Task<bool> ActualizarEmpleadoAsync(int id, CrearEmpleadoDTOs dto)
        {
            var empleadoExistente = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(id);
            if (empleadoExistente == null) return false;

            empleadoExistente.Nombre = dto.Nombre;
            empleadoExistente.Apellido = dto.Apellido;
            empleadoExistente.Categoria = dto.Categoria;
            empleadoExistente.Telefono = dto.Telefono;

            await _empleadoRepository.ActualizarEmpleadoAsync(empleadoExistente);
            return true;
        }

        public async Task<bool> EliminarEmpleadoAsync(int id)
        {
            var empleadoExistente = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(id);
            if (empleadoExistente == null) return false;

            await _empleadoRepository.EliminarEmpleadoAsync(empleadoExistente);
            return true;
        }

        public async Task<bool> EliminarAsignacionEmpleadoObraAsync(int idObra, int idEmpleado)
        {
            var asignacionExistente = await _empleadoRepository.ObtenerAsignacionPorIdAsync(idObra, idEmpleado);
            if (asignacionExistente == null) return false;
            await _empleadoRepository.EliminarAsignacionEmpleadoObraAsync(asignacionExistente);
            return true;
        }




        public async Task<bool> AsignarEmpleadoAObraAsync(AsignarEmpleadoObraDto dto)
{
                
            var empleado = await _empleadoRepository.ObtenerEmpleadoPorIdAsync(dto.IdEmpleado);
            if (empleado == null) { return false; }

            var asignacionExistente = await _empleadoRepository.ObtenerAsignacionPorIdAsync(dto.IdObra, dto.IdEmpleado);
            if (asignacionExistente != null && asignacionExistente.RolEnObra == dto.RolEnObra)
            {
                return false; 
                 
            }
            bool yaTieneObra = await _empleadoRepository.ExisteEmpleadoEnCualquierObraAsync(dto.IdEmpleado);

            if (yaTieneObra)
            {
                return false; // Bloqueado: Los no-supervisores solo pueden estar en 1 obra a la vez
            }

            var asignacion = new EmpleadoObra
    {
        IdEmpleado = dto.IdEmpleado,
        IdObra = dto.IdObra,
        RolEnObra = dto.RolEnObra,
        ValorHoraAsignado = dto.ValorHoraAsignado > 0 ? dto.ValorHoraAsignado : empleado.ValorHora
    };

    await _empleadoRepository.AsignarEmpleadoAObraAsync(asignacion);
    return true;
}

    }
}
