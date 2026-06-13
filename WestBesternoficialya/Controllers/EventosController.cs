using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize]
public class EventosController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- EL TABLERO DE ANUNCIOS (Pantalla principal) ---
    public async Task<IActionResult> Index()
    {
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    // --- VIAJE DE IDA: Formulario para publicar un aviso ---
    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        return View();
    }

    // --- VIAJE DE VUELTA: Guardar el aviso nuevo ---
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create(Evento evento)
    {
        if (ModelState.IsValid)
        {
            evento.Creador = User.Identity.Name;
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction("ExitoAlCrear", new { id = evento.Id });
        }
        return View(evento);
    }

    // ==========================================
    // SECCIÓN: PANTALLA PUENTE (Botón de Continuar)
    // ==========================================
    [Authorize(Roles = "Administrador")]
    public IActionResult ExitoAlCrear(int id)
    {
        ViewBag.EventoId = id;
        return View();
    }

    // ==========================================
    // SECCIÓN: VER DETALLES DEL AVISO Y ALIMENTOS
    // ==========================================
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();

        // Buscamos alimentos y memorandum
        ViewBag.Alimentos = await _context.NotificacionesEventos.FirstOrDefaultAsync(n => n.EventoId == id);
        ViewBag.Memorandum = await _context.Memorandums.FirstOrDefaultAsync(m => m.EventoId == id);

        // ¡LISTO! Eliminamos la carga de mantenimiento de aquí
        return View(evento);
    }

    // ==========================================
    // SECCIÓN: EDITAR EVENTOS Y ALIMENTOS
    // ==========================================
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos
            .Include(e => e.Memorandum)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (evento == null) return NotFound();

        ViewBag.Alimentos = await _context.NotificacionesEventos.FirstOrDefaultAsync(n => n.EventoId == id);

        return View(evento);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int id, Evento evento)
    {
        if (id != evento.Id) return NotFound();

        ModelState.Remove("AcusesRecibo");
        ModelState.Remove("NotificacionesEventos");

        if (ModelState.IsValid)
        {
            _context.Update(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(evento);
    }

    // ==========================================
    // SECCIÓN: ELIMINAR EVENTO (PROTEGIDO)
    // ==========================================
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
        if (evento == null) return NotFound();

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        TempData["MensajeExito"] = "El formato se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ==========================================
    // SECCIÓN: FIRMA DIGITAL (ACUSE DE RECIBO)
    // ==========================================
    public async Task<IActionResult> Firmar(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        return View(evento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarFirma(int eventoId)
    {
        var nombreEnGafete = User.Identity.Name;

        var usuario = await _context.Usuarios
            .Include(u => u.Departamento)
            .FirstOrDefaultAsync(u => u.NombreCompleto == nombreEnGafete);

        if (usuario == null) return Unauthorized();

        bool yaFirmado = await _context.AcusesRecibo
            .AnyAsync(a => a.EventoId == eventoId && a.UsuarioId == usuario.Id);

        if (yaFirmado)
        {
            TempData["MensajeInfo"] = "Ya habías firmado este aviso de logística anteriormente.";
            return RedirectToAction(nameof(Index));
        }

        var acuse = new AcuseRecibo
        {
            EventoId = eventoId,
            UsuarioId = usuario.Id,
            FechaHoraFirma = DateTime.Now,
            DepartamentoFirma = usuario.Departamento?.Nombre ?? "Sin Departamento"
        };

        _context.AcusesRecibo.Add(acuse);
        await _context.SaveChangesAsync();

        TempData["MensajeExito"] = "Tu firma se registró correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ==========================================
    // SECCIÓN: REPORTE DE FIRMAS (QUIÉN YA LEYÓ)
    // ==========================================
    public async Task<IActionResult> ReporteFirmas(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        var firmas = await _context.AcusesRecibo
            .Include(a => a.Usuario)
            .Where(a => a.EventoId == id)
            .OrderByDescending(a => a.FechaHoraFirma)
            .ToListAsync();

        ViewBag.TituloEvento = evento.Titulo;

        return View(firmas);
    }
}