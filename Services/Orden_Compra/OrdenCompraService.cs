using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Materiales;
using MiWebApi.Repository.Orden_Compra;
using MiWebApi.Services.Orden_Compra;
 

namespace MiWebApi.Services.OrdenesCompra
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IOrdenCompraRepository _ordenRepository;
        private readonly IMaterialRepository _materialRepository;

        public OrdenCompraService(
            IOrdenCompraRepository ordenRepository,
            IMaterialRepository materialRepository)
        {
            _ordenRepository = ordenRepository;
            _materialRepository = materialRepository;
        }

        public async Task<IEnumerable<OrdenCompraDTO>> ListarTodasAsync()
        {
            var ordenes = await _ordenRepository.ListarTodasAsync();

            return ordenes.Select(o => new OrdenCompraDTO
            {
                IdOrden = o.IdOrden,
                IdProveedor = o.IdProveedor,
                NombreProveedor = o.IdProveedorNavigation.Nombre,
                FechaPedido = o.FechaPedido,
                MontoTotal = o.MontoTotal,
                EstadoEntrega = o.EstadoEntrega,

                Detalles = o.DetalleOrdenCompras.Select(d => new DetalleOrdenCompraDTO
                {
                    IdMaterial = d.IdMaterial,
                    NombreMaterial = d.IdMaterialNavigation.Nombre,
                    CantidadPedida = d.CantidadPedida,
                    PrecioUnitarioCompra = d.PrecioUnitarioCompra
                }).ToList()
            });
        }


        public async Task<OrdenCompra?> ObtenerPorIdAsync(int id)
        {
            return await _ordenRepository.ObtenerPorIdAsync(id);
        }



        public async Task<bool> CrearAsync(CrearOrdenCompraDTO dto)
        {
            var nuevaOrden = new OrdenCompra
            {
                IdProveedor = dto.IdProveedor,
                FechaPedido = dto.FechaPedido,
                EstadoEntrega = "Pendiente",
                MontoTotal = dto.Detalles.Sum(d => d.CantidadPedida * d.PrecioUnitarioCompra),

                DetalleOrdenCompras = dto.Detalles.Select(d => new DetalleOrdenCompra
                {
                    IdMaterial = d.IdMaterial,
                    CantidadPedida = d.CantidadPedida,
                    PrecioUnitarioCompra = d.PrecioUnitarioCompra
                }).ToList()
            };

            await _ordenRepository.AgregarAsync(nuevaOrden);

            return true;
        }
        public async Task<bool> ActualizarAsync(int idOrden, OrdenCompraDTO dto)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(idOrden);

            if (orden == null)
                return false;

            if (orden.EstadoEntrega == "Entregado")
                return false;

            orden.IdProveedor = dto.IdProveedor;
            orden.FechaPedido = dto.FechaPedido;
            orden.EstadoEntrega = dto.EstadoEntrega;
            orden.MontoTotal = dto.Detalles.Sum(d => d.Subtotal);

            orden.DetalleOrdenCompras.Clear();

            foreach (var detalle in dto.Detalles)
            {
                orden.DetalleOrdenCompras.Add(new DetalleOrdenCompra
                {
                    IdMaterial = detalle.IdMaterial,
                    CantidadPedida = detalle.CantidadPedida,
                    PrecioUnitarioCompra = detalle.PrecioUnitarioCompra
                });
            }

            return await _ordenRepository.ActualizarAsync(orden);
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
                return false;

            return await _ordenRepository.EliminarAsync(orden);
        }

        public async Task<bool> ConfirmarEntregaAsync(int id)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(id);
            if (orden == null)
                return false;
            orden.EstadoEntrega = "Entregado";
            return await _ordenRepository.ActualizarAsync(orden);
        }





    }
}