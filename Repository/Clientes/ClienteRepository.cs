using MiWebApi.Data;
using MiWebApi.Entity;
using Microsoft.EntityFrameworkCore;    

namespace MiWebApi.Repository.Clientes
{
    public class ClienteRepository : IClienteRepository 
    {
        private readonly DbcontruContext _context;
        public ClienteRepository(DbcontruContext context)
        {
            _context = context;
        }


        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> ListarTodosLosClientesAsync()
        {


            return await _context.Clientes.ToListAsync();


        }

        public async Task<bool> AgregarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }   

        public async Task<bool> EliminarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            
        }




    }
