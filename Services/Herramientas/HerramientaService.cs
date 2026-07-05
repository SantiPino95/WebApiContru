using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Dtos;
using MiWebApi.Entity;
using MiWebApi.Repository.Herramientas;

namespace MiWebApi.Services.Herramientas
{
    public class HerramientaService : IHerramientaService
    {
        private readonly IHerramientaRepository _herramientaRepository;
        private readonly DbcontruContext _context;

        public HerramientaService(IHerramientaRepository herramientaRepository , DbcontruContext context)
        {
            _herramientaRepository = herramientaRepository;
            _context = context;

        }

        public async Task<IEnumerable<HerramientaListadoDto>> ObtenerHerramientasParaAdminAsync()
        {
            try
            {
                var listaEntidades = await _herramientaRepository.ListarTodasLasHerramientasAsync();

                Console.WriteLine($"✅ Herramientas encontradas en BD: {listaEntidades.Count()}");

                return listaEntidades.Select(h =>
                {
                    var ultimaAsignacion = h.HerramientasAsignada
                        .OrderByDescending(a => a.FechaSalida)
                        .FirstOrDefault();

                    var estaAsignada = ultimaAsignacion != null && ultimaAsignacion.FechaDevolucion == null;

                    var estadoReal = estaAsignada ? "En Obra" :
                                     h.EstadoDisponibilidad == "En Reparación" ? "En Reparación" :
                                     "Disponible";

                    return new HerramientaListadoDto
                    {
                        IdHerramienta = h.IdHerramienta,
                        NombreTipo = h.NombreTipo,
                        CodigoInventario = h.CodigoInventario,
                        EstadoDisponibilidad = estadoReal,
                        Origen = h.Origen,
                        IdObraActual = ultimaAsignacion?.IdObra,
                        NombreObraActual = ultimaAsignacion?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                        UltimaFechaSalida = ultimaAsignacion?.FechaSalida,
                        FechaDevolucion = ultimaAsignacion?.FechaDevolucion
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR en ObtenerHerramientasParaAdminAsync: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<HerramientaListadoDto?> ObtenerHerramientaPorIdAsync(int id)
        {
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(id);
            if (herramienta == null) return null;

            var ultimaAsignacion = herramienta.HerramientasAsignada
                .OrderByDescending(a => a.FechaSalida)
                .FirstOrDefault();

            var estaAsignada = ultimaAsignacion != null && ultimaAsignacion.FechaDevolucion == null;
            var estadoReal = estaAsignada ? "En Obra" :
                             herramienta.EstadoDisponibilidad == "En Reparación" ? "En Reparación" :
                             "Disponible";

            return new HerramientaListadoDto
            {
                IdHerramienta = herramienta.IdHerramienta,
                NombreTipo = herramienta.NombreTipo,
                CodigoInventario = herramienta.CodigoInventario,
                EstadoDisponibilidad = estadoReal,
                Origen = herramienta.Origen,
                IdObraActual = ultimaAsignacion?.IdObra,
                NombreObraActual = ultimaAsignacion?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                UltimaFechaSalida = ultimaAsignacion?.FechaSalida,
                FechaDevolucion = ultimaAsignacion?.FechaDevolucion
            };
        }

        public async Task<bool> AgregarHerramientaAsync(CrearHerramientaDto dto)
        {
            var nuevaHerramienta = new Herramienta
            {
                NombreTipo = dto.NombreTipo,
                CodigoInventario = dto.CodigoInventario,
                EstadoDisponibilidad = "Disponible",
                Origen = dto.Origen
            };
            return await _herramientaRepository.AgregarHerramientaAsync(nuevaHerramienta);
        }

        public async Task<bool> ActualizarHerramientaAsync(int id, CrearHerramientaDto dto)
        {
            var herramientaExistente = await _herramientaRepository.ObtenerHerramientaPorIdAsync(id);
            if (herramientaExistente == null) return false;

            herramientaExistente.NombreTipo = dto.NombreTipo;
            herramientaExistente.CodigoInventario = dto.CodigoInventario;
            herramientaExistente.Origen = dto.Origen;

            return await _herramientaRepository.ActualizarHerramientaAsync(herramientaExistente);
        }

        // ✅ ELIMINAR - CORREGIDO: Solo permite eliminar si NO está asignada
        public async Task<bool> EliminarHerramientaAsync(int id)
        {
            // 1. Obtener la herramienta con sus asignaciones
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(id);
            if (herramienta == null) return false;

            // 2. Verificar si tiene asignaciones ACTIVAS (sin devolución)
            var tieneAsignacionActiva = herramienta.HerramientasAsignada
                .Any(a => a.FechaDevolucion == null);

            if (tieneAsignacionActiva)
            {
                // ❌ No se puede eliminar si está asignada a una obra
                return false;
            }

            // 3. ✅ ELIMINAR TODAS LAS ASIGNACIONES (aunque estén devueltas)
            //    Esto es necesario porque la relación es REQUIRED
            var asignaciones = herramienta.HerramientasAsignada.ToList();

            if (asignaciones.Any())
            {
                foreach (var asignacion in asignaciones)
                {
                    _context.HerramientasAsignadas.Remove(asignacion);
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ Eliminadas {asignaciones.Count} asignaciones de la herramienta {id}");
            }

            // 4. Ahora eliminar la herramienta (ya no tiene hijos)
            return await _herramientaRepository.EliminarHerramientaAsync(herramienta);
        }

        // ✅ ASIGNAR - CORREGIDO
        public async Task<bool> RegistrarSalidaAsignacionAsync(int idHerramienta, int idObra)
        {
            // 1. Obtener herramienta
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(idHerramienta);
            if (herramienta == null) return false;

            // 2. ✅ Verificar si la herramienta YA ESTÁ ASIGNADA A ALGUNA OBRA
            var asignacionActiva = herramienta.HerramientasAsignada
                .Any(a => a.FechaDevolucion == null);

            if (asignacionActiva)
            {
                // ❌ La herramienta ya está asignada a otra obra
                Console.WriteLine($"❌ Herramienta {idHerramienta} ya está asignada a otra obra");
                return false;
            }

            // 3. Verificar que no esté en reparación
            if (herramienta.EstadoDisponibilidad == "En Reparación") return false;

            // 4. Crear la asignación
            var asignacion = new HerramientasAsignada
            {
                IdHerramienta = idHerramienta,
                IdObra = idObra,
                FechaSalida = DateTime.Now.Date,
                FechaDevolucion = null
            };

            // 5. Guardar la asignación
            var resultado = await _herramientaRepository.RegistrarSalidaAsignacionAsync(asignacion);
            if (!resultado) return false;

            // 6. Actualizar el estado
            herramienta.EstadoDisponibilidad = "En Obra";
            await _herramientaRepository.ActualizarHerramientaAsync(herramienta);

            return true;
        }

        // ✅ DEVOLVER - CORREGIDO
        public async Task<bool> RegistrarDevolucionHerramientaAsync(int idHerramienta, int idObra)
        {
            // 1. Buscar la asignación activa
            var asignacionActiva = await _herramientaRepository.ObtenerAsignacionActivaAsync(idHerramienta, idObra);
            if (asignacionActiva == null) return false;

            // 2. Registrar la devolución
            asignacionActiva.FechaDevolucion = DateTime.Now.Date;
            var resultado = await _herramientaRepository.ActualizarAsignacionAsync(asignacionActiva);
            if (!resultado) return false;

            // 3. ✅ ACTUALIZAR EL ESTADO DE LA HERRAMIENTA A "Disponible"
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(idHerramienta);
            if (herramienta == null) return false;

            herramienta.EstadoDisponibilidad = "Disponible";
            var actualizado = await _herramientaRepository.ActualizarHerramientaAsync(herramienta);

            return actualizado;
        }
    }
}