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
    // ==========================================
    // SECCIÓN: VER DETALLES DEL AVISO / FORMATO
    // ==========================================
    // GET: Eventos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        // Si la URL no trae un ID, mandamos error 404
        if (id == null)
        {
            return NotFound();
        }

        // Buscamos el evento en la base de datos usando el ID
        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);

        // Si no encontramos el aviso en la base de datos, mandamos error 404
        if (evento == null)
        {
            return NotFound();
        }

        // Si todo está bien, mandamos el evento a la vista Details.cshtml
        return View(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        if (ModelState.IsValid)
        {
            // Guardamos de forma segura el NombreCompleto de la sesión activa
            evento.Creador = User.Identity.Name;

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

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // ¡ELIMINAMOS la línea de ViewBag.Usuarios! 
        // Ya no llevamos la charola con los nombres, el candado [Authorize] hará la magia.

        return View(evento);
    }
    // ==========================================
    // SECCIÓN: EDITAR EVENTO (PROTEGIDO)
    // ==========================================

    // 1. VIAJE DE IDA: Comprobamos si el usuario tiene permiso de ver la pantalla de edición
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // VALIDACIÓN DE SEGURIDAD: Si no eres el creador, te regresamos al tablero
        if (evento.Creador != User.Identity.Name)
        {
            TempData["MensajeInfo"] = "No tienes permisos para editar este aviso. Solo el creador (" + evento.Creador + ") puede modificarlo.";
            return RedirectToAction(nameof(Index));
        }

        return View(evento);
    }

    // 2. VIAJE DE VUELTA: Validamos de nuevo en el servidor por si intentan hackear la petición HTTP
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Evento evento)
    {
        if (id != evento.Id) return NotFound();

        // Buscamos el registro real de la base de datos sin rastrearlo en memoria temporalmente
        var eventoOriginal = await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (eventoOriginal == null) return NotFound();

        // Doble verificación de seguridad
        if (eventoOriginal.Creador != User.Identity.Name)
        {
            TempData["MensajeInfo"] = "Acceso denegado: No eres el propietario de este aviso.";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            // Aseguramos que el creador original no se borre ni se modifique al actualizar
            evento.Creador = eventoOriginal.Creador;

            _context.Update(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(evento);
    }

    // ==========================================
    // SECCIÓN: ELIMINAR EVENTO (PROTEGIDO)
    // ==========================================

    // 1. VIAJE DE IDA: Muestra la pantalla de confirmación si eres el dueño del aviso
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
        if (evento == null) return NotFound();

        // VALIDACIÓN DE SEGURIDAD: Si no eres el creador, te regresamos al tablero
        if (evento.Creador != User.Identity.Name)
        {
            TempData["MensajeInfo"] = "No tienes permisos para eliminar este aviso. Solo el creador (" + evento.Creador + ") puede borrarlo.";
            return RedirectToAction(nameof(Index));
        }

        return View(evento);
    }

    // 2. VIAJE DE VUELTA: Elimina el registro de la base de datos tras validar la sesión
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] // Protección esencial contra ataques CSRF en formularios
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);

        if (evento == null) return NotFound();

        // DOBLE VERIFICACIÓN DE SEGURIDAD: Evita hackeos directos por peticiones HTTP post
        if (evento.Creador != User.Identity.Name)
        {
            TempData["MensajeInfo"] = "Acceso denegado: No eres el propietario de este aviso.";
            return RedirectToAction(nameof(Index));
        }

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        TempData["MensajeExito"] = "El aviso se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarFirma(int eventoId)
    {
        // 1. Extraemos el dato del gafete de la sesión (que gracias a tu AccesoController, sabemos que es el Nombre Completo)
        var nombreEnGafete = User.Identity.Name;

        // 2. Buscamos al usuario comparando contra la columna correcta: NombreCompleto
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