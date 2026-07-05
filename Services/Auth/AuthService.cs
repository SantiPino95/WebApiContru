using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiWebApi.Data;
using MiWebApi.DTOs;
using MiWebApi.Entity;
using BCrypt.Net;

namespace MiWebApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DbcontruContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DbcontruContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            // 1. Verificar si el email ya está registrado
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuarioExistente != null)
                return false;

            // 2. Hashear la contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Crear el usuario SOLO (sin empleado)
            var nuevoUsuario = new Usuario
            {
                Email = dto.Email,
                Contrasena = hashedPassword,
                IdRol = 5, // Rol por defecto: Empleado
                Estado = "Pendiente" // Pendiente de aprobación
            };

            await _context.Usuarios.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            // 1. Buscar usuario por email
            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return null;

            // 2. Verificar si el usuario está activo
            if (usuario.Estado != "Activo")
                return null;

            // 3. Verificar contraseña
            bool passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Contrasena);
            if (!passwordValida)
                return null;

            // 4. Generar Token JWT
            var token = GenerarToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Email = usuario.Email,
                NombreCompleto = usuario.Empleado != null
                    ? $"{usuario.Empleado.Nombre} {usuario.Empleado.Apellido}"
                    : "Sin nombre",
                Rol = usuario.IdRolNavigation?.NombreRol ?? "Sin Rol",
                IdUsuario = usuario.IdUsuario,
                Expira = DateTime.UtcNow.AddHours(8)
            };
        }

        private string GenerarToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(
                _configuration["Jwt:Key"] ?? "MiClaveSuperSecretaParaJWT1234567890!");

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.IdRolNavigation?.NombreRol ?? "Empleado"),
                new Claim("Estado", usuario.Estado ?? "Pendiente")
            };

            // Si tiene empleado asociado, agregamos su nombre
            if (usuario.Empleado != null)
            {
                claims.Add(new Claim("Nombre", usuario.Empleado.Nombre));
                claims.Add(new Claim("Apellido", usuario.Empleado.Apellido));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _configuration["Jwt:Issuer"] ?? "MiWebApi",
                Audience = _configuration["Jwt:Audience"] ?? "MiWebApiClient",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}