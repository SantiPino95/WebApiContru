 
using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Entity;

namespace MiWebApi.Repository.Materiales
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly DbcontruContext _materialesContext;

        public MaterialRepository(DbcontruContext context)
        {
            _materialesContext = context;
        }

        public async Task<IEnumerable<Material>> ListarTodosLosMaterialesAsync()
        {
            return await _materialesContext.Material
                .Include(m => m.StockMaterial)
                .ToListAsync();
        }

        public async Task<Material?> ObtenerMaterialPorIdAsync(int id)
        {
            return await _materialesContext.Material
                .Include(m => m.StockMaterial)
                .FirstOrDefaultAsync(m => m.IdMaterial == id);
        }

        public async Task<bool> AgregarMaterialAsync(Material material, StockMaterial stock)
        {
            await _materialesContext.Material.AddAsync(material);
            await _materialesContext.SaveChangesAsync(); // Guardamos para generar el IdMaterial autoincremental

            stock.IdMaterial = material.IdMaterial; // Le pasamos el ID generado a la tabla de Stock
            await _materialesContext.StockMaterial.AddAsync(stock); // Ajustar nombre según tu DBContext

            return await _materialesContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarMaterialAsync(Material material)
        {
            _materialesContext.Material.Update(material);
            return await _materialesContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarMaterialAsync(Material material)
        {
            _materialesContext.Material.Remove(material);
            return await _materialesContext.SaveChangesAsync() > 0;
        }

        // --- MÉTODOS DE LA TABLA INTERMEDIA Y STOCK ---

        public async Task<bool> RegistrarConsumoMaterialAsync(MaterialObra consumo)
        {
            await _materialesContext.MaterialObras.AddAsync(consumo); // Ajustar nombre según tu DBContext
            return await _materialesContext.SaveChangesAsync() > 0;
        }

        public async Task<StockMaterial?> ObtenerStockPorMaterialIdAsync(int idMaterial)
        {
            return await _materialesContext.StockMaterial.FirstOrDefaultAsync(s => s.IdMaterial == idMaterial);
        }

        public async Task<bool> ActualizarStockAsync(StockMaterial stock)
        {
            _materialesContext.StockMaterial.Update(stock);
            return await _materialesContext.SaveChangesAsync() > 0;
        }
    }
}
