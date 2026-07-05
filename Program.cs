using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiWebApi.Data;
using MiWebApi.Repository.Clientes;
using MiWebApi.Repository.Empleados;
using MiWebApi.Repository.Herramientas;
using MiWebApi.Repository.Materiales;
using MiWebApi.Repository.Novedades;
using MiWebApi.Repository.Obras;
using MiWebApi.Repository.PagosProveedores;
using MiWebApi.Repository.Presupuestos;
using MiWebApi.Repository.Proveedores;
using MiWebApi.Repository.Usuarios;
using MiWebApi.Repository.Seguimientos;
using MiWebApi.Services.Auth;
using MiWebApi.Services.Clientes;
using MiWebApi.Services.Empleados;
using MiWebApi.Services.Herramientas;
using MiWebApi.Services.Materiales;
using MiWebApi.Services.Novedades;
using MiWebApi.Services.Obras;
using MiWebApi.Services.PagosProveedores;
using MiWebApi.Services.Presupuestos;
using MiWebApi.Services.Proveedores;
using MiWebApi.Services.Usuarios;
using MiWebApi.Services.Seguimientos;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE SERVICIOS (Contenedor)
// ==========================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Base de datos (MySQL)
builder.Services.AddDbContext<DbcontruContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
);

// Inyección de Dependencias (Agrupado por entidad para mayor orden)
// Clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Empleados
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

// Herramientas
builder.Services.AddScoped<IHerramientaRepository, HerramientaRepository>();
builder.Services.AddScoped<IHerramientaService, HerramientaService>();

// Materiales
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialService, MaterialService>();

// Novedades
builder.Services.AddScoped<INovedadRepository, NovedadRepository>();
builder.Services.AddScoped<INovedadService, NovedadService>();

// Obras
builder.Services.AddScoped<IObraRepository, ObraRepository>();
builder.Services.AddScoped<IObraService, ObraService>();

// Pagos Proveedores
builder.Services.AddScoped<IPagoProveedorRepository, PagoProveedorRepository>();
builder.Services.AddScoped<IPagoProveedorService, PagoProveedorService>();

// Presupuestos
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();

// Proveedores
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

// Seguimientos
builder.Services.AddScoped<ISeguimientoRepository, SeguimientoRepository>();
builder.Services.AddScoped<ISeguimientoService, SeguimientoService>();

// Usuarios y Autenticación
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Autenticación JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "MiClaveSuperSecretaParaJWT1234567890!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MiWebApi",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "MiWebApiClient",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


// ==========================================
// 2. CONSTRUCCIÓN DE LA APP
// ==========================================
var app = builder.Build();


// ==========================================
// 3. PIPELINE DE MIDDLEWARES (¡El orden importa!)
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi Web API V1");
    });
}

// 1° Redirección a HTTPS
app.UseHttpsRedirection();

// 2° CORS (Debe ir antes de la Autenticación y las Rutas para evitar bloqueos)
app.UseCors("PermitirTodo");

// 3° Autenticación (Saber quién es el usuario)
app.UseAuthentication();

// 4° Autorización (Saber a qué tiene permiso)
app.UseAuthorization();

// 5° Mapeo de Controladores (Ejecutar las rutas)
app.MapControllers();

app.Run();
