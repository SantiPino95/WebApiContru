using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Presupuestos;

namespace MiWebApi.Services.Presupuestos
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IPresupuestoRepository _repo;
        public PresupuestoService(IPresupuestoRepository repo) => _repo = repo;

        public async Task<IEnumerable<PresupuestoListadoDto>> ObtenerPresupuestosAsync()
        {
            var entidades = await _repo.ListarTodosConObrasAsync();
            return entidades.Select(p => new PresupuestoListadoDto
            {
                IdPresupuesto = p.IdPresupuesto,
                IdObra = p.IdObra,
                NombreObra = p.IdObraNavigation?.NombreObra ?? "Sin Obra",
                FechaEmision = p.FechaEmision,
                MontoTotal = p.MontoTotal,
                EstadoPresupuesto = p.EstadoPresupuesto,
                CantidadItems = p.DetallePresupuestos.Count
            }).ToList();
        }

        public async Task<bool> CrearPresupuestoAsync(CrearPresupuestoDto dto)
        {
            if (dto.Lineas == null || !dto.Lineas.Any()) return false; // Un presupuesto no puede estar vacío

            // 1. Instanciamos la cabecera
            var nuevoPresupuesto = new Presupuesto
            {
                IdObra = dto.IdObra,
                FechaEmision = DateTime.Now,
                EstadoPresupuesto = dto.EstadoPresupuesto,
                MontoTotal = 0 // Lo calculamos abajo
            };

            decimal acumuladorTotal = 0;

            // 2. Mapeamos y calculamos cada renglón del detalle
            foreach (var lineaDto in dto.Lineas)
            {
                var subtotalLinea = lineaDto.Cantidad * lineaDto.PrecioUnitario;
                acumuladorTotal += subtotalLinea;

                var detalleEntidad = new DetallePresupuesto
                {
                    Descripcion = lineaDto.Descripcion,
                    Cantidad = lineaDto.Cantidad,
                    PrecioUnitario = lineaDto.PrecioUnitario,
                    Subtotal = subtotalLinea
                };

                // Agregamos el detalle a la colección de la cabecera
                nuevoPresupuesto.DetallePresupuestos.Add(detalleEntidad);
            }

            // 3. Asignamos el monto calculado definitivo
            nuevoPresupuesto.MontoTotal = acumuladorTotal;

            // 4. Mandamos todo al repositorio
            return await _repo.AgregarPresupuestoCompletoAsync(nuevoPresupuesto);
        }
    }
}
