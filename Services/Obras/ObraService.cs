using MiWebApi.Models;
using MiWebApi.Repository;

namespace MiWebApi.Services
{
    public class ObraService : IObraService
    {
        private readonly IObraRepository _repo;

        public ObraService(IObraRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Obra>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Obra> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task<Obra> CreateAsync(Obra obra) => await _repo.AddAsync(obra);

        public async Task<Obra> UpdateAsync(int id, Obra obra)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;
            obra.Id = id;
            return await _repo.UpdateAsync(obra);
        }

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}

