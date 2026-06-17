using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize] // <-- ¡ESTE ES EL CANDADO! Nadie entra sin gafete.
public class HomeController : Controller
{
    // ... el resto de tu código ...
    private readonly ApplicationDbContext _context;

    // Le damos al Gerente (Controlador) las llaves de la base de datos
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // 2. Va y cuenta cuántos productos están en alerta roja (Cantidad <= Stock Minimo)
        int alertasAlmacen = await _context.Inventario.CountAsync(p => p.CantidadActual <= p.StockMinimo);

        // 3. Cuenta cuántos empleados tenemos en total
        int totalEmpleados = await _context.Usuarios.CountAsync();

        // 4. Cuenta cuántos anuncios hay en el tablero
        int totalEventos = await _context.Eventos.CountAsync();

        // Ponemos los números en la "charola" (ViewBag) para mandarlos a la pantalla
        ViewBag.AlertasAlmacen = alertasAlmacen;
        ViewBag.TotalEmpleados = totalEmpleados;
        ViewBag.TotalEventos = totalEventos;

        return View();
    }

    // Dejamos la página de privacidad por si la necesitas después
    public IActionResult Privacy()
    {
        return View();
    }
}