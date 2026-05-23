using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization; // <-- No olvides esta línea hasta arriba de todo el archivo

namespace WestBesternoficialya.Controllers;

[Authorize] // <-- ¡ESTE ES EL CANDADO! Nadie entra sin gafete.
public class HabitacionesController : Controller
{
    // ... el resto de tu código ...
    private readonly ApplicationDbContext _context;

    public HabitacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- EL TABLERO DE CONTROL (INDEX) ---
    public async Task<IActionResult> Index()
    {
        var habitaciones = await _context.Habitaciones.ToListAsync();
        return View(habitaciones);
    }

    // --- CREAR HABITACIÓN (GET) ---
    public IActionResult Create()
    {
        return View();
    }

    // --- CREAR HABITACIÓN (POST) ---
    [HttpPost]
    public async Task<IActionResult> Create(Habitacion habitacion)
    {
        if (ModelState.IsValid)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(habitacion);
    }

    // --- EDITAR ESTADO O DETALLES (GET) ---
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var habitacion = await _context.Habitaciones.FindAsync(id);
        if (habitacion == null) return NotFound();

        return View(habitacion);
    }

    // --- EDITAR ESTADO O DETALLES (POST) ---
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Habitacion habitacion)
    {
        if (id != habitacion.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(habitacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(habitacion);
    }

    // --- ELIMINAR (GET) ---
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var habitacion = await _context.Habitaciones.FirstOrDefaultAsync(m => m.Id == id);
        if (habitacion == null) return NotFound();

        return View(habitacion);
    }

    // --- ELIMINAR (POST) ---
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var habitacion = await _context.Habitaciones.FindAsync(id);
        if (habitacion != null)
        {
            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}