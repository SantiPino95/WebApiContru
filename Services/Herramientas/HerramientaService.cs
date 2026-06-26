using MiWebApi.Dtos;
using MiWebApi.Repository.Herramientas;
using MiWebApi.Entity;
using System.Security.Cryptography.X509Certificates;

namespace MiWebApi.Services.Herramientas
{
    public class HerramientaService : IHerramientaService
    {


        private readonly IHerramientaRepository _herramientaRepository;


        public HerramientaService(IHerramientaRepository herramientaRepository)
        {
            _herramientaRepository = herramientaRepository;
        }


        public async Task<IEnumerable<HerramientaListadoDto>> ObtenerHerramientasParaAdminAsync()
        {
            // Usamos el nombre exacto de tu método del repositorio: ListarTodasLasHerramientasAsync()
            var listaEntidades = await _herramientaRepository.ListarTodasLasHerramientasAsync();

            return listaEntidades.Select(h =>
            {
                var ultimaAsignacion = h.HerramientasAsignada
                    .OrderByDescending(a => a.FechaSalida)
                    .FirstOrDefault();

                return new HerramientaListadoDto
                {
                    IdHerramienta = h.IdHerramienta,
                    NombreTipo = h.NombreTipo,
                    CodigoInventario = h.CodigoInventario,
                    EstadoDisponibilidad = h.EstadoDisponibilidad,
                    Origen = h.Origen,

                    IdObraActual = ultimaAsignacion?.IdObra,
                    NombreObraActual = ultimaAsignacion?.IdObraNavigation?.NombreObra ?? "Sin Obra",
                    UltimaFechaSalida = ultimaAsignacion?.FechaSalida,
                    FechaDevolucion = ultimaAsignacion?.FechaDevolucion
                };
            }).ToList();
        }


        public async Task<HerramientaListadoDto?> ObtenerHerramientaPorIdAsync(int id)
        {
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(id);
            if (herramienta == null) return null;
            var ultimaAsignacion = herramienta.HerramientasAsignada
                .OrderByDescending(a => a.FechaSalida)
                .FirstOrDefault();
            return new HerramientaListadoDto
            {
                IdHerramienta = herramienta.IdHerramienta,
                NombreTipo = herramienta.NombreTipo,
                CodigoInventario = herramienta.CodigoInventario,
                EstadoDisponibilidad = herramienta.EstadoDisponibilidad,
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
                EstadoDisponibilidad = dto.EstadoDisponibilidad,
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
            herramientaExistente.EstadoDisponibilidad = dto.EstadoDisponibilidad;
            herramientaExistente.Origen = dto.Origen;
            return await _herramientaRepository.ActualizarHerramientaAsync(herramientaExistente);
        }



        public async Task<bool> EliminarHerramientaAsync(int id)
        {
            var herramientaExistente = await _herramientaRepository.ObtenerHerramientaPorIdAsync(id);
            if (herramientaExistente == null) return false;
            return await _herramientaRepository.EliminarHerramientaAsync(herramientaExistente);





        }

        public async Task<bool> RegistrarSalidaAsignacionAsync(int idHerramienta, int idObra)
        {
            var asignacion = new HerramientasAsignada
            {
                IdHerramienta = idHerramienta,
                IdObra = idObra,
                FechaSalida = DateTime.Now,
                FechaDevolucion = null // Inicialmente null, se actualizará cuando se devuelva la herramienta
            };
            return await _herramientaRepository.RegistrarSalidaAsignacionAsync(asignacion);
        }


        public async Task<bool> RegistrarDevolucionHerramientaAsync(int idHerramienta, int idObra)
        {
            // 1. Buscamos la asignación que está abierta en la base de datos (FechaDevolucion es null)
            var asignacionActiva = await _herramientaRepository.ObtenerAsignacionActivaAsync(idHerramienta, idObra);

            if (asignacionActiva == null) return false; // Si no existe, no hay nada que devolver

            // 2. EL SERVICIO LE PONE LA FECHA ACTUAL (Nadie digita nada a mano)
            asignacionActiva.FechaDevolucion = DateTime.Now;

            // 3. LLAMAMOS AL REPOSITORIO (Acá es donde se usa el update de la tabla intermedia)
            var asignacionActualizada = await _herramientaRepository.ActualizarAsignacionAsync(asignacionActiva);
            if (!asignacionActualizada) return false;

            // 4. Cambiamos el estado de la herramienta fuerte a "Disponible" para que se pueda volver a usar
            var herramienta = await _herramientaRepository.ObtenerHerramientaPorIdAsync(idHerramienta);
            if (herramienta != null)
            {
                herramienta.EstadoDisponibilidad = "Disponible";
                await _herramientaRepository.ActualizarHerramientaAsync(herramienta);
            }

            return true;
        }
    }
}

