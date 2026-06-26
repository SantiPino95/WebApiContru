using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Herramientas
{
    public class HerramientaRepository : IHerramientaRepository
    {
        private readonly DbcontruContext _herramientaContext;

        public HerramientaRepository(DbcontruContext herramientaContext)
        {
            _herramientaContext = herramientaContext;
        }


        public async Task<IEnumerable<Herramienta>> ListarTodasLasHerramientasAsync()
        {
            return await _herramientaContext.Herramientas.Include(h => h.HerramientasAsignada).ThenInclude(a => a.IdObraNavigation).ToListAsync();
        }

        public async Task<Herramienta?> ObtenerHerramientaPorIdAsync(int id)
        {
            return await _herramientaContext.Herramientas.FindAsync(id);
        }

        public async Task<bool> AgregarHerramientaAsync(Herramienta herramienta)
        {
            await _herramientaContext.Herramientas.AddAsync(herramienta);
            return await _herramientaContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarHerramientaAsync(Herramienta herramienta)
        {
            _herramientaContext.Herramientas.Update(herramienta);
            return await _herramientaContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarHerramientaAsync(Herramienta herramienta)
        {
            _herramientaContext.Herramientas.Remove(herramienta);
            return await _herramientaContext.SaveChangesAsync() > 0;
           
        }

        public async Task<bool> RegistrarSalidaAsignacionAsync(HerramientasAsignada asignacion)
        {
            await _herramientaContext.HerramientasAsignadas.AddAsync(asignacion);
            return await _herramientaContext.SaveChangesAsync() > 0;
        }

        public async Task<HerramientasAsignada?> ObtenerAsignacionActivaAsync(int idHerramienta, int idObra)
        {
            // Buscamos el registro donde la herramienta está en la obra y todavía NO fue devuelta (FechaDevolucion es null)
            return await _herramientaContext.HerramientasAsignadas
                .FirstOrDefaultAsync(a => a.IdHerramienta == idHerramienta
                                       && a.IdObra == idObra
                                       && a.FechaDevolucion == null);
        }

        public async Task<bool> ActualizarAsignacionAsync(HerramientasAsignada asignacion)
        {
            _herramientaContext.HerramientasAsignadas.Update(asignacion);
            return await _herramientaContext.SaveChangesAsync() > 0;
        }



    }
}
