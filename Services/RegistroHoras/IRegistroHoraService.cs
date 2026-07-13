using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.RegistroHoras;

namespace MiWebApi.Services.RegistroHoras
{
    public interface IRegistroHoraService
    {
        Task<IEnumerable<RegistroHoraDto>> ObtenerPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes);
        Task<RegistroHoraDto?> ObtenerPorIdAsync(int idRegistro);

        // Devuelve: (éxito, mensaje de error si corresponde, dto creado)
        Task<(bool Exito, string? Error, RegistroHoraDto? Dto)> CrearAsync(int idEmpleado, CrearRegistroHoraDto dto);
        Task<(bool Exito, string? Error)> ActualizarAsync(int idEmpleado, int idRegistro, ActualizarRegistroHoraDto dto);
    }

    public class RegistroHoraService : IRegistroHoraService
    {
        private readonly IRegistroHoraRepository _repository;

        public RegistroHoraService(IRegistroHoraRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RegistroHoraDto>> ObtenerPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes)
        {
            var registros = await _repository.ListarPorEmpleadoYMesAsync(idEmpleado, anio, mes);
            return registros.Select(MapearADto);
        }

        public async Task<RegistroHoraDto?> ObtenerPorIdAsync(int idRegistro)
        {
            var registro = await _repository.ObtenerPorIdAsync(idRegistro);
            return registro == null ? null : MapearADto(registro);
        }

        public async Task<(bool Exito, string? Error, RegistroHoraDto? Dto)> CrearAsync(int idEmpleado, CrearRegistroHoraDto dto)
        {
            // 1. La asignación (Id_Empleado_Obra) tiene que existir y ser del empleado que hace el pedido
            var empleadoObra = await _repository.ObtenerEmpleadoObraAsync(dto.IdEmpleadoObra);
            if (empleadoObra == null || empleadoObra.IdEmpleado != idEmpleado)
            {
                return (false, "La asignación indicada no es válida para este empleado.", null);
            }

            // 2. No permitir dos registros el mismo día para la misma asignación
            var existente = await _repository.ObtenerPorEmpleadoObraYFechaAsync(dto.IdEmpleadoObra, dto.Fecha);
            if (existente != null)
            {
                return (false, "Ya existe un registro de horas para ese día. Usá la edición en su lugar.", null);
            }

            var nuevoRegistro = new RegistroHora
            {
                IdEmpleadoObra = dto.IdEmpleadoObra,
                Fecha = dto.Fecha.Date,
                HorasComunes = dto.HorasComunes,
                HorasExtras = dto.HorasExtras,
                ObservacionesEmpleado = dto.ObservacionesEmpleado
            };

            await _repository.AgregarAsync(nuevoRegistro);

            // Recargamos con el navigation incluido para devolver el DTO completo
            var creado = await _repository.ObtenerPorIdAsync(nuevoRegistro.IdRegistro);
            return (true, null, creado == null ? null : MapearADto(creado));
        }

        public async Task<(bool Exito, string? Error)> ActualizarAsync(int idEmpleado, int idRegistro, ActualizarRegistroHoraDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(idRegistro);
            if (registro == null)
            {
                return (false, "No se encontró el registro.");
            }

            // Verificamos que el registro le pertenezca al empleado que pide la edición
            if (registro.IdEmpleadoObraNavigation.IdEmpleado != idEmpleado)
            {
                return (false, "No tenés permiso para editar este registro.");
            }

            registro.HorasComunes = dto.HorasComunes;
            registro.HorasExtras = dto.HorasExtras;
            registro.ObservacionesEmpleado = dto.ObservacionesEmpleado;

            await _repository.ActualizarAsync(registro);
            return (true, null);
        }

        private static RegistroHoraDto MapearADto(RegistroHora r) => new()
        {
            IdRegistro = r.IdRegistro,
            IdEmpleadoObra = r.IdEmpleadoObra,
            IdObra = r.IdEmpleadoObraNavigation.IdObra,
            NombreObra = r.IdEmpleadoObraNavigation.IdObraNavigation.NombreObra,
            Fecha = r.Fecha,
            HorasComunes = r.HorasComunes,
            HorasExtras = r.HorasExtras,
            ObservacionesEmpleado = r.ObservacionesEmpleado
        };
    }
}