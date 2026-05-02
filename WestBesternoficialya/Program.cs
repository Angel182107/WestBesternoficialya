using Microsoft.EntityFrameworkCore;
// OJO: Si le pusiste otro nombre a tu proyecto, cambia "HotelSanPedroWeb" por tu nombre exacto
using WestBesternoficialya.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. NUESTRO CÓDIGO PARA MYSQL EMPIEZA AQUÍ ---
// Leemos las "llaves" desde tu archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Le decimos al preparador que agregue la base de datos a sus herramientas
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// --- NUESTRO CÓDIGO TERMINA AQUÍ ---

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();