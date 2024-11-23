using ClinicaMedica.Data;
using ClinicaMedica.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexi�n desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar DbContext para Entity Framework Core
builder.Services.AddDbContext<ClinicaMedicaDbContext>(options =>
    options.UseSqlServer(connectionString));

// Habilitar servicios de sesi�n
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de inactividad antes de expirar
    options.Cookie.HttpOnly = true; // Seguridad para cookies de sesi�n
    options.Cookie.IsEssential = true; // Obligatorio para que funcione sin consentimiento de cookies
});

// Agregar servicios para controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline de la aplicaci�n
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // P�gina de error en producci�n
    app.UseHsts(); // Seguridad HSTS en producci�n
}

app.UseHttpsRedirection(); // Redirigir solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Habilitar archivos est�ticos (wwwroot)

app.UseRouting();

app.UseSession(); // Habilitar sesiones
app.UseAuthorization(); // Middleware de autorizaci�n

// Configurar rutas de controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();
