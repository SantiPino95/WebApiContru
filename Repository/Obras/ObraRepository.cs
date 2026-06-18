using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Models;

namespace MiWebApi.Repository
{
    public class ObraRepository : IObraRepository
    {
        private readonly AppDbContext _ctx;

        public ObraRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Obra>> GetAllAsync()
        {
            return await _ctx.Obras.ToListAsync();
        }

        public async Task<Obra> GetByIdAsync(int id)
        {
            return await _ctx.Obras.FindAsync(id);
        }

        public async Task<Obra> AddAsync(Obra obra)
        {
            _ctx.Obras.Add(obra);
            await _ctx.SaveChangesAsync();
            return obra;
        }

        public async Task<Obra> UpdateAsync(Obra obra)
        {
            _ctx.Obras.Update(obra);
            await _ctx.SaveChangesAsync();
            return obra;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var o = await _ctx.Obras.FindAsync(id);
            if (o == null) return false;
            _ctx.Obras.Remove(o);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
