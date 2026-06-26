 
using MiWebApi.Dtos;
using MiWebApi.Entity;
using MiWebApi.Repository.Materiales;

namespace MiWebApi.Services.Materiales
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<IEnumerable<MaterialListadoDto>> ObtenerMaterialesParaAdminAsync()
        {
            var entidades = await _materialRepository.ListarTodosLosMaterialesAsync();
            return entidades.Select(m => new MaterialListadoDto
            {
                IdMaterial = m.IdMaterial,
                Nombre = m.Nombre,
                UnidadMedida = m.UnidadMedida,
                CantidadDisponible = m.StockMaterial?.CantidadDisponible ?? 0,
                StockMinimo = m.StockMaterial?.StockMinimo ?? 0
            }).ToList();
        }

        public async Task<bool> AgregarMaterialAsync(CrearMaterialDto dto)
        {
            var nuevoMaterial = new Material
            {
                Nombre = dto.Nombre,
                UnidadMedida = dto.UnidadMedida
            };

            var nuevoStock = new StockMaterial
            {
                CantidadDisponible = dto.CantidadInicial,
                StockMinimo = dto.StockMinimo
            };

            return await _materialRepository.AgregarMaterialAsync(nuevoMaterial, nuevoStock);
        }

        public async Task<bool> ActualizarMaterialAsync(int id, CrearMaterialDto dto)
        {
            var existente = await _materialRepository.ObtenerMaterialPorIdAsync(id);
            if (existente == null) return false;

            existente.Nombre = dto.Nombre;
            existente.UnidadMedida = dto.UnidadMedida;

            if (existente.StockMaterial != null)
            {
                existente.StockMaterial.StockMinimo = dto.StockMinimo; // Permitimos actualizar el mínimo acá
            }

            return await _materialRepository.ActualizarMaterialAsync(existente);
        }

        public async Task<bool> EliminarMaterialAsync(int id)
        {
            var existente = await _materialRepository.ObtenerMaterialPorIdAsync(id);
            if (existente == null) return false;

            return await _materialRepository.EliminarMaterialAsync(existente);
        }

        public async Task<bool> RegistrarConsumoEnObraAsync(int idMaterial, int idObra, decimal cantidad)
        {
            // 1. Validar que tengamos stock suficiente para mandar a la obra
            var stock = await _materialRepository.ObtenerStockPorMaterialIdAsync(idMaterial);
            if (stock == null || stock.CantidadDisponible < cantidad) return false; // Stock insuficiente

            // 2. Descontar del stock
            stock.CantidadDisponible -= cantidad;
            await _materialRepository.ActualizarStockAsync(stock);

            // 3. Registrar el consumo en la tabla intermedia
            var consumo = new MaterialObra
            {
                IdMaterial = idMaterial,
                IdObra = idObra,
                CantidadConsumida = cantidad,
                FechaConsumo = DateTime.Now
            };

            return await _materialRepository.RegistrarConsumoMaterialAsync(consumo);
        }
    }
}
