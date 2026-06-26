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
            var listaEntidades = await _empleadoRepository.ListarTodosLosEmpleadosAsync();

            return listaEntidades.Select(e => new EmpleadoListadoDTOs
            {
                IdEmpleado = e.IdEmpleado,
                // Combinamos nombre y apellido directamente aquí en C#
                NombreCompleto = $"{e.Nombre} {e.Apellido}",
                Categoria = e.Categoria,
                Telefono = e.Telefono
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

        public async Task<bool> CrearEmpleadoAsync(CrearEmpleadoDTOs dto)
        {
            var nuevoEmpleado = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Categoria = dto.Categoria,
                Telefono = dto.Telefono
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


    }
}
