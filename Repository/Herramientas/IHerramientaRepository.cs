using System.Collections.Generic;
using System.Threading.Tasks;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Herramientas
{
    public interface IHerramientaRepository
    {
         
        Task<IEnumerable<Herramienta>> ListarTodasLasHerramientasAsync();

        
        Task<Herramienta?> ObtenerHerramientaPorIdAsync(int idHerramienta);

        
        Task<bool> AgregarHerramientaAsync(Herramienta herramienta);
        Task<bool> ActualizarHerramientaAsync(Herramienta herramienta);
        Task<bool> EliminarHerramientaAsync(Herramienta herramienta);


        Task<bool> RegistrarSalidaAsignacionAsync(HerramientasAsignada asignacion);
        Task<HerramientasAsignada?> ObtenerAsignacionActivaAsync(int idHerramienta, int idObra);
        Task<bool> ActualizarAsignacionAsync(HerramientasAsignada asignacion);
    }

}