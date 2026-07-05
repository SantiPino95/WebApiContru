using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.PagosProveedores;

namespace MiWebApi.Services.PagosProveedores
{
    public class PagoProveedorService : IPagoProveedorService
    {
        private readonly IPagoProveedorRepository _pagoRepository;

        public PagoProveedorService(IPagoProveedorRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }

        public async Task<IEnumerable<PagoProveedorListadoDto>> ObtenerTodosAsync()
        {
            var entidades = await _pagoRepository.ListarTodosAsync();

            return entidades.Select(p => new PagoProveedorListadoDto
            {
                IdPagoProveedor = p.IdPagoProveedor,
                IdProveedor = p.IdProveedor,
                NombreProveedor = p.IdProveedorNavigation?.Nombre ?? "Sin Proveedor",
                FechaPago = p.FechaPago,
                Monto = p.Monto,
                MetodoPago = p.MetodoPago
            }).ToList();
        }

        public async Task<PagoProveedorListadoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _pagoRepository.ObtenerPorIdAsync(id);
            if (entidad == null) return null;

            return new PagoProveedorListadoDto
            {
                IdPagoProveedor = entidad.IdPagoProveedor,
                IdProveedor = entidad.IdProveedor,
                NombreProveedor = entidad.IdProveedorNavigation?.Nombre ?? "Sin Proveedor",
                FechaPago = entidad.FechaPago,
                Monto = entidad.Monto,
                MetodoPago = entidad.MetodoPago
            };
        }

        public async Task<IEnumerable<PagoProveedorListadoDto>> ObtenerPorProveedorAsync(int idProveedor)
        {
            var entidades = await _pagoRepository.ListarPorProveedorAsync(idProveedor);

            return entidades.Select(p => new PagoProveedorListadoDto
            {
                IdPagoProveedor = p.IdPagoProveedor,
                IdProveedor = p.IdProveedor,
                NombreProveedor = p.IdProveedorNavigation?.Nombre ?? "Sin Proveedor",
                FechaPago = p.FechaPago,
                Monto = p.Monto,
                MetodoPago = p.MetodoPago
            }).ToList();
        }

        public async Task<bool> CrearPagoAsync(CrearPagoProveedorDto dto)
        {
            var pago = new PagoProveedor
            {
                IdProveedor = dto.IdProveedor,
                FechaPago = dto.FechaPago,
                Monto = dto.Monto,
                MetodoPago = dto.MetodoPago
            };

            return await _pagoRepository.AgregarAsync(pago);
        }

        public async Task<bool> EliminarPagoAsync(int id)
        {
            var pago = await _pagoRepository.ObtenerPorIdAsync(id);
            if (pago == null) return false;

            return await _pagoRepository.EliminarAsync(pago);
        }
    }
}