using Backend.Setup;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "Files"
});

// Configuración básica
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Configuración de Swagger (sin seguridad JWT)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistema de Gestion de Inventarios", Version = "v1" });
});

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configuración de límites
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104_857_600; // 100MB
});

// Configuración de infraestructura (sin autenticación JWT)
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

// Configuración de FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configuración adicional
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de archivos estáticos
var uploadDirectory = Path.Combine(
    app.Environment.IsDevelopment() ?
        Directory.GetCurrentDirectory() :
        builder.Configuration["StaticFilesPath"] ?? Directory.GetCurrentDirectory(),
    "Files");

if (!Directory.Exists(uploadDirectory))
{
    Directory.CreateDirectory(uploadDirectory);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadDirectory),
    RequestPath = "/Files"
});

// Middlewares
app.UseCors("Angular");
app.UseHttpsRedirection();
// Se elimina app.UseAuthentication();
app.UseAuthorization();

// Middlewares personalizados
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.Run();