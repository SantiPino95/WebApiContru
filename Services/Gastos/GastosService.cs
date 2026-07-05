using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Gastos;

namespace MiWebApi.Services.Gastos
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastoRepository;

        public GastoService(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        public async Task<IEnumerable<GastoListadoDto>> ObtenerTodosAsync()
        {
            var entidades = await _gastoRepository.ListarTodosAsync();
            return entidades.Select(g => new GastoListadoDto
            {
                IdGasto = g.IdGasto,
                IdObra = g.IdObra,
                NombreObra = g.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = g.Fecha,
                Monto = g.Monto,
                Descripcion = g.Descripcion,
                CategoriaGasto = g.CategoriaGasto,
                NroComprobante = g.NroComprobante
            }).ToList();
        }

        public async Task<GastoListadoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _gastoRepository.ObtenerPorIdAsync(id);
            if (entidad == null) return null;

            return new GastoListadoDto
            {
                IdGasto = entidad.IdGasto,
                IdObra = entidad.IdObra,
                NombreObra = entidad.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = entidad.Fecha,
                Monto = entidad.Monto,
                Descripcion = entidad.Descripcion,
                CategoriaGasto = entidad.CategoriaGasto,
                NroComprobante = entidad.NroComprobante
            };
        }

        public async Task<IEnumerable<GastoListadoDto>> ObtenerPorObraAsync(int idObra)
        {
            var entidades = await _gastoRepository.ListarPorObraAsync(idObra);
            return entidades.Select(g => new GastoListadoDto
            {
                IdGasto = g.IdGasto,
                IdObra = g.IdObra,
                NombreObra = g.IdObraNavigation?.NombreObra ?? "Sin Obra",
                Fecha = g.Fecha,
                Monto = g.Monto,
                Descripcion = g.Descripcion,
                CategoriaGasto = g.CategoriaGasto,
                NroComprobante = g.NroComprobante
            }).ToList();
        }

        public async Task<bool> CrearGastoAsync(CrearGastoDto dto)
        {
            var gasto = new GastosObra
            {
                IdObra = dto.IdObra,
                Fecha = dto.Fecha,
                Monto = dto.Monto,
                Descripcion = dto.Descripcion,
                CategoriaGasto = dto.CategoriaGasto,
                NroComprobante = dto.NroComprobante
            };

            return await _gastoRepository.AgregarAsync(gasto);
        }

        public async Task<bool> ActualizarGastoAsync(int id, ActualizarGastoDto dto)
        {
            var gasto = await _gastoRepository.ObtenerPorIdAsync(id);
            if (gasto == null) return false;

            gasto.Fecha = dto.Fecha;
            gasto.Monto = dto.Monto;
            gasto.Descripcion = dto.Descripcion;
            gasto.CategoriaGasto = dto.CategoriaGasto;
            gasto.NroComprobante = dto.NroComprobante;

            return await _gastoRepository.ActualizarAsync(gasto);
        }

        public async Task<bool> EliminarGastoAsync(int id)
        {
            var gasto = await _gastoRepository.ObtenerPorIdAsync(id);
            if (gasto == null) return false;

            return await _gastoRepository.EliminarAsync(gasto);
        }
    }
}