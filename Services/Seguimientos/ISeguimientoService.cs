using MiWebApi.DTOs;

namespace MiWebApi.Services.Seguimientos
{
    public interface ISeguimientoService
    {
        Task<IEnumerable<SeguimientoListadoDto>> ListarPorObraAsync(int idObra);
        Task<SeguimientoListadoDto?> ObtenerUltimoPorObraAsync(int idObra);

        // Retorna una tupla con Exito y Mensaje para informar si se pasó del 100%
        Task<(bool Exito, string Mensaje)> RegistrarAvanceAsync(CrearSeguimientoDto dto);

        Task<bool> EliminarAsync(int idSeguimiento);
    }
}