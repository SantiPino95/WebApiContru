using Microsoft.EntityFrameworkCore;
using MiWebApi.Data;
using MiWebApi.Repository.Clientes;
using MiWebApi.Repository.Empleados;
using MiWebApi.Repository.Obras;
using MiWebApi.Repository.Herramientas; 
using MiWebApi.Repository.Materiales;
using MiWebApi.Services.Clientes;
using MiWebApi.Services.Empleados;
using MiWebApi.Services.Obras;
using MiWebApi.Services.Herramientas;
using MiWebApi.Services.Materiales;

var builder = WebApplication.CreateBuilder(args);

// --- AGREGAR SERVICIOS AL CONTENEDOR ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Tu Base de datos (MySQL Oficial de Oracle)
builder.Services.AddDbContext<DbcontruContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("DefaultConnection")?? string.Empty
    ));

// ¡TUS CLASES NUEVAS!
builder.Services.AddScoped<IObraRepository, ObraRepository>();
builder.Services.AddScoped<IObraService, ObraService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IHerramientaRepository, HerramientaRepository>();
builder.Services.AddScoped<IHerramientaService, HerramientaService>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialService, MaterialService>();


// ----------------------------------------
var app = builder.Build();

// --- CONFIGURAR EL PIPELINE HTTP (Middlewares) ---
// Todo lo relacionado a Swagger lo metemos dentro del mismo IF de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Esto asegura que la interfaz se cargue correctamente en la raíz o en /swagger
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi Web API V1");
    });
}
app.UseCors("PermitirTodo");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();