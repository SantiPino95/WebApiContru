
using Microsoft.EntityFrameworkCore;
using MiWebApi.DTOs;
using MiWebApi.Repository.Obras;
using MiWebApi.Data;
using MiWebApi.Entity;
using System.Linq;


namespace MiWebApi.Services.Obras
{
    public class ObraService : IObraService
    {
        private readonly IObraRepository _obraRepository;
        private readonly DbcontruContext _context;
        public ObraService(IObraRepository obraRepository, DbcontruContext context)
        {
            _obraRepository = obraRepository;
            _context = context;
        }



        // 1. Agregamos async y Task
        public async Task<IEnumerable<ObraAdminListadoDto>> ObtenerObrasParaAdminAsync()
        {
            // 1. Viaja a la BD (Ahora trae Clientes, Gastos y Seguimientos gracias a los Includes del Repo)
            var listaEntidades = await _obraRepository.ListarTodasLasObrasAsync();

            // 2. Transformamos la lista a DTOs en memoria
            return listaEntidades.Select(o =>
            {
                // Buscamos el seguimiento más reciente para saber el % actual de la obra
                var ultimoSeguimiento = o.SeguimientoObras
                    .OrderByDescending(s => s.Fecha)
                    .ThenByDescending(s => s.IdSeguimiento)
                    .FirstOrDefault();

                return new ObraAdminListadoDto
                {
                    IdObra = o.IdObra,
                    NombreObra = o.NombreObra,
                    Direccion = o.Direccion,
                    FechaInicio = o.FechaInicio,
                    FechaFinPrevista = o.FechaFinPrevista,
                    Estado = o.Estado,
                    IdCliente = o.IdCliente,
                    NombreCliente = o.IdClienteNavigation != null ? o.IdClienteNavigation.Nombre : "Sin Cliente",
                    CodigoFormateado = $"#{o.CodigoPublico}",

                    // --- NUEVOS CAMPOS CALCULADOS AL VUELO ---
                    // Sumamos todos los montos de la lista de gastos de esta obra
                    TotalGastado = o.GastosObras.Sum(g => g.Monto),

                    // Si hay seguimientos, mostramos el último porcentaje, si no, arranca en 0%
                    PorcentajeAvanceActual = ultimoSeguimiento?.PorcentajeAvance ?? 0
                };
            }).ToList();
        }




        public async Task<ObraDetalleDto?> ObtenerObraPorIdAsync(int idObra)
        {
            var obra = await _obraRepository.ObtenerObraPorIdAsync(idObra);
            if (obra == null)
                return null;
            return new ObraDetalleDto
            {
                IdObra = obra.IdObra,
                NombreObra = obra.NombreObra,
                Direccion = obra.Direccion,
                FechaInicio = obra.FechaInicio,
                FechaFinPrevista = obra.FechaFinPrevista,
                Estado = obra.Estado,
                IdCliente = obra.IdCliente,
                NombreCliente = obra.IdClienteNavigation != null ? obra.IdClienteNavigation.Nombre : "Sin Cliente",
                CodigoFormateado = $"#{obra.CodigoPublico}",

                EmpleadosAsignados = obra.EmpleadoObras.Select(eo => new EmpleadoListadoDTOs
                {
                    IdEmpleado = eo.IdEmpleadoNavigation.IdEmpleado,
                    NombreCompleto = $"{eo.IdEmpleadoNavigation.Nombre} {eo.IdEmpleadoNavigation.Apellido}",
                    Categoria = eo.IdEmpleadoNavigation.Categoria,
                    Telefono = eo.IdEmpleadoNavigation.Telefono
                }).ToList()
            };

        }

        public async Task<bool> EliminarObraAsync(int idObra)
        {
            var obraExistente = await _obraRepository.ObtenerObraPorIdAsync(idObra);
            if (obraExistente == null)
                return false;
            obraExistente.Estado = "Eliminada";

            await _obraRepository.ActualizarObraAsync(obraExistente);
            return true;


        }

        public async Task<bool> CrearObraAsync(CrearObraDto dto)
        {
            var nuevaObra = new Entity.Obra
            {
                IdCliente = dto.IdCliente,
                NombreObra = dto.NombreObra,
                Direccion = dto.Direccion,
                FechaInicio = dto.FechaInicio,
                FechaFinPrevista = dto.FechaFinPrevista,
                Estado = "En Progreso",
                CodigoPublico = Guid.NewGuid().ToString().Substring(0, 10).ToUpper()
            };
            await _obraRepository.AgregarObraAsync(nuevaObra);
            return true;


        }
        public async Task<bool> ActualizarObraAsync(int id, ObraAdminListadoDto dto)
        {
            // 1. Buscamos la obra incluyendo su relación de Seguimiento
            // (Asegúrate de que tu repositorio o consulta use .Include(o => o.Seguimiento))
            var obraBD = await _context.Obras
                .Include(o => o.SeguimientoObras) // 🚀 Ajusta "Seguimiento" al nombre exacto de la propiedad en tu entidad Obra
                .FirstOrDefaultAsync(o => o.IdObra == id);

            if (obraBD == null) return false;

            // 2. Actualizamos los datos básicos de la obra
            obraBD.NombreObra = dto.NombreObra;
            obraBD.Direccion = dto.Direccion;
            obraBD.Estado = dto.Estado; // Actualizamos el estado ('En Progreso', 'Finalizada', etc.)

            if (obraBD.SeguimientoObras != null && obraBD.SeguimientoObras.Any())
            {
                // Buscamos el último registro de seguimiento que se haya creado para esta obra
                var ultimoSeguimiento = obraBD.SeguimientoObras.LastOrDefault(); // O .FirstOrDefault() según cómo los ordenes

                if (ultimoSeguimiento != null)
                {
                    
                    ultimoSeguimiento.PorcentajeAvance = dto.PorcentajeAvanceActual;

                    _context.Entry(ultimoSeguimiento).State = EntityState.Modified;
                }
            }
            else
            {
                // Si la lista está vacía o es nula, inicializamos la colección y creamos el primer registro
                if (obraBD.SeguimientoObras == null)
                {
                    obraBD.SeguimientoObras = new List<SeguimientoObra>();
                }

                var nuevoSeguimiento = new SeguimientoObra
                {
                    IdObra = id, // Revisa si en tu entidad se llama IdObra o Id_Obra
                    PorcentajeAvance = dto.PorcentajeAvanceActual
                    // ... agrega aquí otras propiedades obligatorias si tu base de datos las pide
                };

                obraBD.SeguimientoObras.Add(nuevoSeguimiento);
            }
            // 4. Guardamos los cambios. EF Core guardará la Obra y el Seguimiento en una sola transacción
            _context.Obras.Update(obraBD);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExisteObraPorCodigoPublicoAsync(string codigoPublico)
        {
            var obra = await _context.Obras.FirstOrDefaultAsync(o => o.CodigoPublico == codigoPublico);
            return obra != null;
        }
    }
}







    

