using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Proveedores;

namespace MiWebApi.Services.Proveedores
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repo;
        public ProveedorService(IProveedorRepository repo) => _repo = repo;

        public async Task<IEnumerable<ProveedorListadoDto>> ObtenerTodosAsync()
        {
            var lista = await _repo.ListarTodosAsync();
            return lista.Select(p => new ProveedorListadoDto
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre,
                Rut = p.Rut,
                Telefono = p.Telefono,
                Email = p.Email
            }).ToList();
        }

        public async Task<bool> CrearProveedorAsync(CrearProveedorDto dto)
        {
            // Regla de Negocio: No permitir RUT repetido en el taller
            var existente = await _repo.ObtenerPorRutAsync(dto.Rut);
            if (existente != null) return false;

            var nuevo = new Proveedor
            {
                Nombre = dto.Nombre,
                Rut = dto.Rut,
                Telefono = dto.Telefono,
                Email = dto.Email
            };
           return  await _repo.AgregarAsync(nuevo);
            
        }


        public async Task<bool> ActualizarProveedorAsync(int id, CrearProveedorDto dto) {
            var existente = await _repo.ObtenerPorIdAsync(id);

           if (existente == null) return false;
            // Regla de Negocio: No permitir RUT repetido en el taller
            var otroConMismoRut = await _repo.ObtenerPorRutAsync(dto.Rut);
            if (otroConMismoRut != null && otroConMismoRut.IdProveedor != id) return false;
            existente.Nombre = dto.Nombre;
            existente.Rut = dto.Rut;
            existente.Telefono = dto.Telefono;
            existente.Email = dto.Email;
            return await _repo.ActualizarAsync(existente);
            

        }
    }
}
