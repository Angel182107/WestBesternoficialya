using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//  Cadena de conexión (de appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🗄 Registro del DbContext con MySQL (Pomelo)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//  Servicios MVC
builder.Services.AddControllersWithViews();
// Le decimos al sistema que use el gafete virtual (Cookies) y que la puerta es /Acceso/Index
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // El gafete dura 1 hora
    });

var app = builder.Build();

//  Manejo de errores (producción)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔐 Middleware básico
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// 🧭 Ruta por defecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();