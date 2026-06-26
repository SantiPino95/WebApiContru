using MiWebApi.Dtos;
using MiWebApi.DTOs;
using MiWebApi.Entity;

namespace MiWebApi.Services.Herramientas
{
    public interface IHerramientaService
    {
        Task <IEnumerable<HerramientaListadoDto>> ObtenerHerramientasParaAdminAsync();
        Task<HerramientaListadoDto?> ObtenerHerramientaPorIdAsync(int id);

        Task<bool> AgregarHerramientaAsync(CrearHerramientaDto dto);
        Task<bool> ActualizarHerramientaAsync(int id , CrearHerramientaDto dto);
        Task<bool> EliminarHerramientaAsync(int id);

        Task <bool> RegistrarSalidaAsignacionAsync(int idHerramienta, int idObra);

        Task<bool> RegistrarDevolucionHerramientaAsync(int idHerramienta, int idObra);

       


    }
}
