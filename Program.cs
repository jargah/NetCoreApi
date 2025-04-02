using CajaMoreliaApi.Middleware;
using CajaMoreliaApi.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<ConnectorDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios de controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


// Agregar Swagger para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Permitir cualquier dominio
              .AllowAnyMethod()   // Permitir cualquier método (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader();  // Permitir cualquier encabezado (Authorization, Content-Type, etc.)
    });
});


// Construir la aplicación
var app = builder.Build();

app.UseCors("AllowAll");


// Middleware para validar la API Key
app.UseMiddleware<Credentials>();

// Configurar Swagger en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Habilitar autorización (si se usa en el futuro)
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();