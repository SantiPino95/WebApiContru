using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Novedades;

namespace MiWebApi.Services.Novedades
{
    public class NovedadService : INovedadService
    {
        private readonly INovedadRepository _novedadRepository;

        public NovedadService(INovedadRepository novedadRepository)
        {
            _novedadRepository = novedadRepository;
        }

        public async Task<IEnumerable<NovedadListadoDto>> ObtenerTodasAsync()
        {
            var entidades = await _novedadRepository.ListarTodasAsync();

            return entidades.Select(n => new NovedadListadoDto
            {
                IdNovedad = n.IdNovedad,
                IdEmpleadoObra = n.IdEmpleadoObra,
                NombreEmpleado = $"{n.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Nombre} {n.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Apellido}" ?? "Sin Empleado",
                IdObra = n.IdEmpleadoObraNavigation?.IdObra ?? 0,
                NombreObra = n.IdEmpleadoObraNavigation?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = n.Fecha,
                TipoNovedad = n.TipoNovedad,
                Descripcion = n.Descripcion,
                EstadoRevision = n.EstadoRevision
            }).ToList();
        }

        public async Task<NovedadListadoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _novedadRepository.ObtenerPorIdAsync(id);
            if (entidad == null) return null;

            return new NovedadListadoDto
            {
                IdNovedad = entidad.IdNovedad,
                IdEmpleadoObra = entidad.IdEmpleadoObra,
                NombreEmpleado = $"{entidad.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Nombre} {entidad.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Apellido}" ?? "Sin Empleado",
                IdObra = entidad.IdEmpleadoObraNavigation?.IdObra ?? 0,
                NombreObra = entidad.IdEmpleadoObraNavigation?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = entidad.Fecha,
                TipoNovedad = entidad.TipoNovedad,
                Descripcion = entidad.Descripcion,
                EstadoRevision = entidad.EstadoRevision
            };
        }

        public async Task<IEnumerable<NovedadListadoDto>> ObtenerPorObraAsync(int idObra)
        {
            var entidades = await _novedadRepository.ListarPorObraAsync(idObra);

            return entidades.Select(n => new NovedadListadoDto
            {
                IdNovedad = n.IdNovedad,
                IdEmpleadoObra = n.IdEmpleadoObra,
                NombreEmpleado = $"{n.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Nombre} {n.IdEmpleadoObraNavigation?.IdEmpleadoNavigation?.Apellido}" ?? "Sin Empleado",
                IdObra = n.IdEmpleadoObraNavigation?.IdObra ?? 0,
                NombreObra = n.IdEmpleadoObraNavigation?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = n.Fecha,
                TipoNovedad = n.TipoNovedad,
                Descripcion = n.Descripcion,
                EstadoRevision = n.EstadoRevision
            }).ToList();
        }

        public async Task<bool> CrearNovedadAsync(CrearNovedadDto dto)
        {
            var novedad = new NovedadesObra
            {
                IdEmpleadoObra = dto.IdEmpleadoObra,
                Fecha = dto.Fecha,
                TipoNovedad = dto.TipoNovedad,
                Descripcion = dto.Descripcion,
                EstadoRevision = dto.EstadoRevision ?? "Pendiente"
            };

            return await _novedadRepository.AgregarAsync(novedad);
        }

        public async Task<bool> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var novedad = await _novedadRepository.ObtenerPorIdAsync(id);
            if (novedad == null) return false;

            var estadosValidos = new[] { "Pendiente", "Revisado", "Resuelto", "Desestimado" };
            if (!estadosValidos.Contains(nuevoEstado)) return false;

            novedad.EstadoRevision = nuevoEstado;
            return await _novedadRepository.ActualizarAsync(novedad);
        }

        public async Task<bool> EliminarNovedadAsync(int id)
        {
            var novedad = await _novedadRepository.ObtenerPorIdAsync(id);
            if (novedad == null) return false;

            return await _novedadRepository.EliminarAsync(novedad);
        }
    }
}