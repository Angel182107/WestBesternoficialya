using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize] // <-- ¡ESTE ES EL CANDADO! Nadie entra sin gafete.
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
        // Traemos todos los eventos guardados en la base de datos
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    // --- VIAJE DE IDA: Formulario para publicar un aviso ---
    // Agregamos el candado para que solo el Admin pueda crear instructivos
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
            // Guardamos de forma segura el NombreCompleto de la sesión activa
            evento.Creador = User.Identity.Name;

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            // EL TRUCO ESTÁ AQUÍ:
            // En vez de mandarte al tablero, te mandamos a una pantalla de "Éxito"
            // y le pasamos el ID del aviso que acabas de crear para que no se pierda.
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
        // Guardamos el número de grupo para usarlo en el siguiente paso (La Notificación de Evento)
        ViewBag.EventoId = id;
        return View();
    }

    // ==========================================
    // SECCIÓN: VER DETALLES DEL AVISO / FORMATO
    // ==========================================
    // ==========================================
    // SECCIÓN: VER DETALLES DEL AVISO Y ALIMENTOS
    // ==========================================
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        // 1. Buscamos el Instructivo de Grupo (El Aviso Principal)
        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);

        if (evento == null) return NotFound();

        // 2. Buscamos el formato de alimentos engrapado a este grupo
        var alimentos = await _context.NotificacionesEventos.FirstOrDefaultAsync(n => n.EventoId == id);

        ViewBag.Memorandum = await _context.Memorandums.FirstOrDefaultAsync(m => m.EventoId == id);

        // 3. Pasamos el formato de alimentos a la pantalla a través de la charola "ViewBag"
        ViewBag.Alimentos = alimentos;

        return View(evento);
    }

    // ==========================================
    // SECCIÓN: EDITAR EVENTOS Y ALIMENTOS
    // ==========================================

    // 1. VIAJE DE IDA: Mostrar la pantalla con los datos llenos
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

    // 2. VIAJE DE VUELTA: Guardar los cambios nuevos
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int id, Evento evento)
    {
        if (id != evento.Id) return NotFound();

        // ESTAS LÍNEAS PERMITEN GUARDAR EL AVISO SIN QUE MARQUE ERROR
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

    // 1. VIAJE DE IDA: Muestra la pantalla de confirmación
    [HttpGet]
    [Authorize(Roles = "Administrador")] // <-- Nuevo candado: Solo Administradores borran
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();

        return View(evento);
    }

    // 2. VIAJE DE VUELTA: Elimina el registro de la base de datos
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] // Protección esencial contra ataques CSRF en formularios
    [Authorize(Roles = "Administrador")] // <-- Nuevo candado de seguridad
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

    // --- VIAJE DE IDA: Mostrar la pantalla para firmar ---
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
        // 1. Extraemos el dato del gafete de la sesión
        var nombreEnGafete = User.Identity.Name;

        // 2. Buscamos al usuario comparando contra la columna correcta
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreCompleto == nombreEnGafete);

        if (usuario == null)
        {
            return Unauthorized();
        }

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
            FechaHoraFirma = DateTime.Now
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

        // 1. Buscamos de qué evento nos están pidiendo el reporte
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // 2. Buscamos todas las firmas en la libreta
        var firmas = await _context.AcusesRecibo
            .Include(a => a.Usuario)
            .Where(a => a.EventoId == id)
            .ToListAsync();

        // 3. Guardamos el título del evento en una "charola" extra
        ViewBag.TituloEvento = evento.Titulo;

        return View(firmas);
    }
}