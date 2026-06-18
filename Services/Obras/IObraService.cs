using MiWebApi.Models;

namespace MiWebApi.Services
{
    public interface IObraService
    {
        Task<IEnumerable<Obra>> GetAllAsync();
        Task<Obra> GetByIdAsync(int id);
        Task<Obra> CreateAsync(Obra obra);
        Task<Obra> UpdateAsync(int id, Obra obra);
        Task<bool> DeleteAsync(int id);
    }
}
