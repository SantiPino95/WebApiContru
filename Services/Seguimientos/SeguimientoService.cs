using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Seguimientos;

namespace MiWebApi.Services.Seguimientos
{
    public class SeguimientoService : ISeguimientoService
    {
        private readonly ISeguimientoRepository _seguimientoRepo;
        private readonly DbcontruContext _context;

        public SeguimientoService(ISeguimientoRepository seguimientoRepo, DbcontruContext context)
        {
            _seguimientoRepo = seguimientoRepo;
            _context = context;
        }

        // 1. Convertimos la lista de Entidades a DTOs
        public async Task<IEnumerable<SeguimientoListadoDto>> ListarPorObraAsync(int idObra)
        {
            var entidades = await _seguimientoRepo.ListarPorObraAsync(idObra);

            return entidades.Select(s => new SeguimientoListadoDto
            {
                IdSeguimiento = s.IdSeguimiento,
                IdObra = s.IdObra,
                PorcentajeAvance = s.PorcentajeAvance,
                DescripcionAvance = s.DescripcionAvance,
                Fecha = s.Fecha
            });
        }

        public async Task<SeguimientoListadoDto?> ObtenerUltimoPorObraAsync(int idObra)
        {
            var s = await _seguimientoRepo.ObtenerUltimoPorObraAsync(idObra);
            if (s == null) return null;

            return new SeguimientoListadoDto
            {
                IdSeguimiento = s.IdSeguimiento,
                IdObra = s.IdObra,
                PorcentajeAvance = s.PorcentajeAvance,
                DescripcionAvance = s.DescripcionAvance,
                Fecha = s.Fecha
            };
        }

        // 2. Registramos validando con DTO
        public async Task<(bool Exito, string Mensaje)> RegistrarAvanceAsync(CrearSeguimientoDto dto)
        {
            var obraBD = await _context.Obras
                .Include(o => o.SeguimientoObras)
                .FirstOrDefaultAsync(o => o.IdObra == dto.IdObra);

            if (obraBD == null)
                return (false, "La obra no existe.");

            // Suma de los avances que ya existen en la BD
            decimal acumuladoActual = obraBD.SeguimientoObras?.Sum(s => s.PorcentajeAvance) ?? 0;

            // 🛑 VALIDACIÓN DE MÁXIMO 100%
            if (acumuladoActual + dto.PorcentajeAvance > 100)
            {
                decimal restante = 100 - acumuladoActual;
                return (false, $"El avance superaría el 100%. Solo queda disponible un {restante}%.");
            }
            if (obraBD.Estado == "Cancelada")
            {
                return (false, "No se pueden registrar avances en una obra que se encuentra Cancelada.");
            }
            if(obraBD.Estado == "Finalizada")
            {
                return (false, "No se pueden registrar avances en una obra que ya ha sido Finalizada.");
            }

            // Mapeamos de DTO a Entidad pura
            var nuevaEntidad = new SeguimientoObra
            {
                IdObra = dto.IdObra,
                PorcentajeAvance = dto.PorcentajeAvance,
                DescripcionAvance = dto.DescripcionAvance,
                Fecha = DateTime.Now
            };

            // Guardamos el seguimiento mediante el repositorio
            await _seguimientoRepo.AgregarAsync(nuevaEntidad);

            // 🚀 ACTUALIZAMOS LA OBRA PRINCIPAL
            decimal nuevoTotal = acumuladoActual + dto.PorcentajeAvance;

            if (nuevoTotal >= 100)
            {
                obraBD.Estado = "Finalizada";
            }
            else if (obraBD.Estado == "Planificada")
            {
                obraBD.Estado = "En Progreso";
            }

            _context.Obras.Update(obraBD);
            await _context.SaveChangesAsync();

            return (true, "Avance registrado con éxito.");
        }

        public async Task<bool> EliminarAsync(int idSeguimiento)
        {
            var entidad = await _context.SeguimientoObras.FindAsync(idSeguimiento);
            if (entidad == null) return false;

            return await _seguimientoRepo.EliminarAsync(entidad);
        }
    }
}