using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Cadena de conexión (de appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🗄️ Registro del DbContext con MySQL (Pomelo)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 🎮 Servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🚨 Manejo de errores (producción)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔐 Middleware básico
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🧭 Ruta por defecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();