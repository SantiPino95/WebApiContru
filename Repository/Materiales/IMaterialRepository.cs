 
using MiWebApi.Entity;

namespace MiWebApi.Repository.Materiales
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> ListarTodosLosMaterialesAsync();
        Task<Material?> ObtenerMaterialPorIdAsync(int id);
        Task<bool> AgregarMaterialAsync(Material material, StockMaterial stock); // Agrega ambos en combo
        Task<bool> ActualizarMaterialAsync(Material material);
        Task<bool> EliminarMaterialAsync(Material material);

        // --- Movimientos / Consumos en Obra (Tabla Intermedia) ---
        Task<bool> RegistrarConsumoMaterialAsync(MaterialObra consumo);
        Task<StockMaterial?> ObtenerStockPorMaterialIdAsync(int idMaterial);
        Task<bool> ActualizarStockAsync(StockMaterial stock);
    }
}