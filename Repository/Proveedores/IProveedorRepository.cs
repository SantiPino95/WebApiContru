using MiWebApi.Entity;

 
    namespace MiWebApi.Repository.Proveedores
    {
        public interface IProveedorRepository
        {
            Task<IEnumerable<Proveedor>> ListarTodosAsync();
            Task<Proveedor?> ObtenerPorIdAsync(int id);
            Task<Proveedor?> ObtenerPorRutAsync(string rut); // Para validación de negocio
            Task<bool> AgregarAsync(Proveedor proveedor);
            Task<bool> ActualizarAsync(Proveedor proveedor);
            Task<bool> EliminarAsync(Proveedor proveedor);
        }
    }
