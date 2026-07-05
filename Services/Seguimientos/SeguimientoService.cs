using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Seguimientos;

namespace MiWebApi.Services.Seguimientos
{
    public class SeguimientoService : ISeguimientoService
    {
        private readonly ISeguimientoRepository _seguimientoRepository;

        public SeguimientoService(ISeguimientoRepository seguimientoRepository)
        {
            _seguimientoRepository = seguimientoRepository;
        }

        public async Task<IEnumerable<SeguimientoListadoDto>> ObtenerPorObraAsync(int idObra)
        {
            var entidades = await _seguimientoRepository.ListarPorObraAsync(idObra);

            return entidades.Select(s => new SeguimientoListadoDto
            {
                IdSeguimiento = s.IdSeguimiento,
                IdObra = s.IdObra,
                NombreObra = s.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = s.Fecha,
                DescripcionAvance = s.DescripcionAvance,
                PorcentajeAvance = s.PorcentajeAvance,
                ImgProgreso = s.ImgProgreso
            }).ToList();
        }

        public async Task<SeguimientoListadoDto?> ObtenerUltimoPorObraAsync(int idObra)
        {
            var entidad = await _seguimientoRepository.ObtenerUltimoPorObraAsync(idObra);
            if (entidad == null) return null;

            return new SeguimientoListadoDto
            {
                IdSeguimiento = entidad.IdSeguimiento,
                IdObra = entidad.IdObra,
                NombreObra = entidad.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = entidad.Fecha,
                DescripcionAvance = entidad.DescripcionAvance,
                PorcentajeAvance = entidad.PorcentajeAvance,
                ImgProgreso = entidad.ImgProgreso
            };
        }

        public async Task<bool> CrearSeguimientoAsync(CrearSeguimientoDto dto)
        {
            // Validar que el porcentaje esté entre 0 y 100
            if (dto.PorcentajeAvance < 0 || dto.PorcentajeAvance > 100)
                return false;

            var seguimiento = new SeguimientoObra
            {
                IdObra = dto.IdObra,
                Fecha = dto.Fecha,
                DescripcionAvance = dto.DescripcionAvance,
                PorcentajeAvance = dto.PorcentajeAvance,
                ImgProgreso = dto.ImgProgreso
            };

            return await _seguimientoRepository.AgregarAsync(seguimiento);
        }

        public async Task<bool> EliminarSeguimientoAsync(int id)
        {
            var seguimiento = await _seguimientoRepository.ObtenerUltimoPorObraAsync(id);
            if (seguimiento == null) return false;

            return await _seguimientoRepository.EliminarAsync(seguimiento);
        }
    }
}