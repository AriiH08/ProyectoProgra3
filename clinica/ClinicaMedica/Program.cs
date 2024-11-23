using ClinicaMedica.Data;
using ClinicaMedica.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar DbContext para Entity Framework Core
builder.Services.AddDbContext<ClinicaMedicaDbContext>(options =>
    options.UseSqlServer(connectionString));

// Habilitar servicios de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de inactividad antes de expirar
    options.Cookie.HttpOnly = true; // Seguridad para cookies de sesión
    options.Cookie.IsEssential = true; // Obligatorio para que funcione sin consentimiento de cookies
});

// Agregar servicios para controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Página de error en producción
    app.UseHsts(); // Seguridad HSTS en producción
}

app.UseHttpsRedirection(); // Redirigir solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Habilitar archivos estáticos (wwwroot)

app.UseRouting();

app.UseSession(); // Habilitar sesiones
app.UseAuthorization(); // Middleware de autorización

// Configurar rutas de controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();
