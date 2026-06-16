using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;
using System.Threading.Tasks;

namespace WestBesternoficialya.Controllers;

[Authorize]
public class EventosController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- TABLERO DE ANUNCIOS ---
    public async Task<IActionResult> Index()
    {
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    // --- PUBLICAR NUEVO AVISO (GET) ---
    [HttpGet]
    [Authorize(Roles = "Administrador, Mantenimiento")]
    public IActionResult Create()
    {
        return View();
    }

    // --- PUBLICAR NUEVO AVISO (POST - RECIBE EL EXCEL) ---
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador, Mantenimiento")]
    public async Task<IActionResult> Create(Evento evento, IFormFile archivoExcel)
    {
        ModelState.Remove("Creador");

        if (archivoExcel != null && archivoExcel.Length > 0)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string nombreUnico = Guid.NewGuid().ToString() + "_" + Path.GetFileName(archivoExcel.FileName);
            string filePath = Path.Combine(folderPath, nombreUnico);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await archivoExcel.CopyToAsync(stream);
            }

            evento.DetallesLogistica = "/uploads/" + nombreUnico;
        }
        else
        {
            ModelState.AddModelError("", "Por favor, selecciona un archivo Excel válido.");
            return View(evento);
        }

        evento.FechaCreacion = DateTime.Now;
        evento.Creador = User.Identity.Name ?? "Administrador";

        _context.Add(evento);
        await _context.SaveChangesAsync();

        TempData["MensajeExito"] = "Aviso publicado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // --- VER DETALLES ---
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();
        return View(evento);
    }

    // --- EDITAR ---
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();
        return View(evento);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int id, Evento evento)
    {
        if (id != evento.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(evento);
    }

    // --- ELIMINAR ---
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();
        return View(evento);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento != null)
        {
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // --- FIRMA DIGITAL ---
    public async Task<IActionResult> Firmar(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _context.Eventos.FindAsync(id);
        return View(evento);
    }

    public async Task<IActionResult> ReporteFirmas(int? id)
    {
        if (id == null) return NotFound();
        var firmas = await _context.AcusesRecibo.Include(a => a.Usuario).Where(a => a.EventoId == id).ToListAsync();
        return View(firmas);
    }
}