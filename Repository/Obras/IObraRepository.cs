using MiWebApi.Models;

namespace MiWebApi.Repository
{
    public interface IObraRepository
    {
        Task<IEnumerable<Obra>> GetAllAsync();
        Task<Obra> GetByIdAsync(int id);
        Task<Obra> AddAsync(Obra obra);
        Task<Obra> UpdateAsync(Obra obra);
        Task<bool> DeleteAsync(int id);
    }
}
