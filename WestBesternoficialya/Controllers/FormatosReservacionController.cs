using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize(Roles = "Administrador")] // Solo el jefe hace cotizaciones financieras
public class FormatosReservacionController : Controller
{
    private readonly ApplicationDbContext _context;

    public FormatosReservacionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- PANTALLA DE CREACIÓN ---
    public IActionResult Create()
    {
        // Traemos todos los "Avisos/Instructivos" de la base de datos para ponerlos en un menú desplegable
        // Así podemos "engrapar" esta cotización a su grupo correspondiente
        ViewBag.EventoId = new SelectList(_context.Eventos, "Id", "Grupo");
        return View();
    }

    // --- GUARDAR EN LA BASE DE DATOS ---
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FormatoReservacion formato)
    {
        if (ModelState.IsValid)
        {
            _context.Add(formato);
            await _context.SaveChangesAsync();

            // Cuando termine de guardar, lo mandamos al tablero principal de Avisos
            return RedirectToAction("Index", "Eventos");
        }

        // Si hay error, recargamos el menú desplegable para que no se rompa la página
        ViewBag.EventoId = new SelectList(_context.Eventos, "Id", "Grupo", formato.EventoId);
        return View(formato);
    }
}