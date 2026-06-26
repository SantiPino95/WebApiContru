using Microsoft.EntityFrameworkCore;    
using MiWebApi.Data;
using MiWebApi.DTOs;
using MiWebApi.Entity;
using MiWebApi.Repository.Clientes;
using System.Security.Cryptography.X509Certificates;

namespace MiWebApi.Services.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // 1. LISTAR TODOS LOS CLIENTES (Igual a la estructura de Obras)
        public async Task<IEnumerable<CLientesListadoDTOs>> ObtenerClientesParaAdminAsync()
        {
            // Primero hacemos el viaje a la base de datos con el await
            var listaEntidades = await _clienteRepository.ListarTodosLosClientesAsync();

            // Luego transformamos a DTO en memoria usando .ToList() común al final
            return listaEntidades.Select(c => new CLientesListadoDTOs
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Email = c.Email,
                Direccion = c.Direccion,
                Obras = c.Obras.Select(o => new ObraResumenDto
                {
                    IdObra = o.IdObra,
                    NombreObra = o.NombreObra
                }).ToList()
            }).ToList();
        }

        // 2. OBTENER UN CLIENTE POR ID (Igual a la estructura de Obras)
        public async Task<CLientesListadoDTOs?> ObtenerClientePorIdAsync(int id)
        {
            // Buscamos la entidad con await al principio
            var cliente = await _clienteRepository.ObtenerClientePorIdAsync(id);
            if (cliente == null)
                return null;

            // Retornamos el DTO transformado directamente
            return new CLientesListadoDTOs
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                Obras = cliente.Obras.Select(o => new ObraResumenDto
                {
                    IdObra = o.IdObra,
                    NombreObra = o.NombreObra
                }).ToList()
            };



        }



        public async Task<bool> CrearClienteAsync(CrearClienteDTOs dto)
        {
            // 1. Creamos la entidad a partir del DTO
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Email = dto.Email
            };

            // 2. Llamamos al repositorio y esperamos a que guarde en la BD
            await _clienteRepository.AgregarClienteAsync(cliente);

            // 3. Si llegó hasta aquí sin errores, devolvemos true
            return true;
        }


        public async Task<bool> ActualizarClienteAsync(int id, CrearClienteDTOs dto)
        {
            // 1. Buscamos el cliente existente
            var clienteExistente = await _clienteRepository.ObtenerClientePorIdAsync(id);
            if (clienteExistente == null)
                return false; // No existe
            // 2. Actualizamos los campos
            clienteExistente.Nombre = dto.Nombre;
            clienteExistente.Direccion = dto.Direccion;
            clienteExistente.Telefono = dto.Telefono;
            clienteExistente.Email = dto.Email;
            // 3. Guardamos los cambios
            await _clienteRepository.ActualizarClienteAsync(clienteExistente);
            return true;
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            // 1. Buscamos el cliente existente
            var clienteExistente = await _clienteRepository.ObtenerClientePorIdAsync(id);
            if (clienteExistente == null)
                return false; // No existe
            // 2. Llamamos al repositorio para eliminarlo
            await _clienteRepository.EliminarClienteAsync(clienteExistente);
            return true;
        }
    }
}
