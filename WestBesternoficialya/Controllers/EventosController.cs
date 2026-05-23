using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization; // <-- No olvides esta línea hasta arriba de todo el archivo

namespace WestBesternoficialya.Controllers;

[Authorize] // <-- ¡ESTE ES EL CANDADO! Nadie entra sin gafete.
public class EventosController : Controller
{
    // ... el resto de tu código ...
    private readonly ApplicationDbContext _context;

    public EventosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- EL TABLERO DE ANUNCIOS (Pantalla principal) ---
    public async Task<IActionResult> Index()
    {
        // Traemos todos los eventos guardados en la base de datos
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    // --- VIAJE DE IDA: Formulario para publicar un aviso ---
    public IActionResult Create()
    {
        return View();
    }

    // --- VIAJE DE VUELTA: Guardar el aviso ---
    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        if (ModelState.IsValid)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(evento);
    }
    // ==========================================
    // SECCIÓN: FIRMA DIGITAL (ACUSE DE RECIBO)
    // ==========================================

    // --- VIAJE DE IDA: Mostrar la pantalla para firmar ---
    public async Task<IActionResult> Firmar(int? id)
    {
        if (id == null) return NotFound();

        // Buscamos cuál es el aviso que quieren firmar
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // Llevamos la charola con la lista de todos los empleados para que elijan su nombre
        ViewBag.Usuarios = new SelectList(_context.Usuarios, "Id", "NombreCompleto");

        return View(evento); // Le pasamos los datos del aviso a la pantalla
    }
    // ==========================================
    // SECCIÓN: EDITAR EVENTO
    // ==========================================
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        return View(evento);
    }

    [HttpPost]
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

    // ==========================================
    // SECCIÓN: ELIMINAR EVENTO
    // ==========================================
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();

        return View(evento);
    }

    [HttpPost, ActionName("Delete")]
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

    // --- VIAJE DE VUELTA: Guardar la firma y la hora exacta ---
    [HttpPost]
    public async Task<IActionResult> GuardarFirma(int eventoId, int usuarioId)
    {
        // Creamos un nuevo registro en la libreta de firmas
        var acuse = new AcuseRecibo
        {
            EventoId = eventoId,
            UsuarioId = usuarioId,
            // ¡La magia! La computadora anota la fecha y hora de este preciso instante:
            FechaHoraFirma = DateTime.Now
        };

        // Lo guardamos en la caja fuerte de la base de datos
        _context.AcusesRecibo.Add(acuse);
        await _context.SaveChangesAsync();

        // Los regresamos al tablero de anuncios
        return RedirectToAction(nameof(Index));
    }
    // ==========================================
    // SECCIÓN: REPORTE DE FIRMAS (QUIÉN YA LEYÓ)
    // ==========================================
    public async Task<IActionResult> ReporteFirmas(int? id)
    {
        if (id == null) return NotFound();

        // 1. Buscamos de qué evento nos están pidiendo el reporte
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // 2. Buscamos todas las firmas en la libreta (AcusesRecibo) que pertenezcan a este evento
        // y traemos los datos del empleado (Usuario) para saber su nombre.
        var firmas = await _context.AcusesRecibo
            .Include(a => a.Usuario)
            .Where(a => a.EventoId == id)
            .ToListAsync();

        // 3. Guardamos el título del evento en una "charola" extra para mostrarlo en la pantalla
        ViewBag.TituloEvento = evento.Titulo;

        return View(firmas);
    }
}