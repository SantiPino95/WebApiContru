using MiWebApi.Data;
using Microsoft.EntityFrameworkCore;    
using MiWebApi.Entity;  
namespace MiWebApi.Repository.Obras
{
    public class ObraRepository : IObraRepository
    {
       private readonly DbcontruContext _context;
        public ObraRepository(DbcontruContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Obra>> ListarTodasLasObrasAsync()
        {
            return await _context.Obras
                .Include(o => o.IdClienteNavigation)
                .Include(o => o.GastosObras)       // <-- Lo incluimos para poder sumar los montos en el DTO
                .Include(o => o.SeguimientoObras)  // <-- Lo incluimos para sacar el último % de avance
                .ToListAsync();
        }

        public async Task<Obra?> ObtenerObraPorIdAsync(int id)
        {
            return await _context.Obras
                .Include(o => o.IdClienteNavigation)
                .Include(o => o.EmpleadoObras)
                    .ThenInclude(eo => eo.IdEmpleadoNavigation)
                .Include(o => o.GastosObras)       // <-- Para ver el desglose de gastos de esta obra
                .Include(o => o.SeguimientoObras)  // <-- Para ver la línea de tiempo de avances
                .SingleOrDefaultAsync(o => o.IdObra == id);
        }




        public async Task AgregarObraAsync(Obra obra)
        {
            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();
        }



        public async Task ActualizarObraAsync(Obra obra)
        {
            _context.Obras.Update(obra);
            await _context.SaveChangesAsync();
        }



        public async Task EliminarObraAsync(Obra obra)
        {
            _context.Obras.Remove(obra);
             await _context.SaveChangesAsync();
            }
        }
    }

               
            