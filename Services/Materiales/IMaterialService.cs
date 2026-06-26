 
using MiWebApi.Dtos;

namespace MiWebApi.Services.Materiales
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialListadoDto>> ObtenerMaterialesParaAdminAsync();
        Task<bool> AgregarMaterialAsync(CrearMaterialDto dto);
        Task<bool> ActualizarMaterialAsync(int id, CrearMaterialDto dto);
        Task<bool> EliminarMaterialAsync(int id);

        // Caso de uso: Mandar materiales a la obra y restar del stock de forma automática
        Task<bool> RegistrarConsumoEnObraAsync(int idMaterial, int idObra, decimal cantidad);
    }
}