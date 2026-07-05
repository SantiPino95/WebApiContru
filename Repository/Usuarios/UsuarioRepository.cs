using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbcontruContext _context;

        public UsuarioRepository(DbcontruContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ListarPendientesAsync()
        {
            return await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.Empleado)
                .Where(u => u.Estado == "Pendiente" || u.Estado == "Pendiente de Aprobación")
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}