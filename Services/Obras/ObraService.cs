
using MiWebApi.DTOs;
using MiWebApi.Repository.Obras;


namespace MiWebApi.Services.Obras
{
    public class ObraService : IObraService
    {
        private readonly IObraRepository _obraRepository;
        public ObraService(IObraRepository obraRepository)
        {
            _obraRepository = obraRepository;
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
                CodigoPublico = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };
            await _obraRepository.AgregarObraAsync(nuevaObra);
            return true;


        }
        public async Task<bool> ActualizarObraAsync(int idObra, CrearObraDto dto)
        {
            var obraExistente = await _obraRepository.ObtenerObraPorIdAsync(idObra);
            if (obraExistente == null)
                return false;
            obraExistente.IdCliente = dto.IdCliente;
            obraExistente.NombreObra = dto.NombreObra;
            obraExistente.Direccion = dto.Direccion;
            obraExistente.FechaInicio = dto.FechaInicio;
            obraExistente.FechaFinPrevista = dto.FechaFinPrevista;
            await _obraRepository.ActualizarObraAsync(obraExistente);
            return true;
        }

       
    }
}







    

