using Microsoft.AspNetCore.Mvc;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize(Roles = "Administrador")] // Candado de seguridad
public class NotificacionesEventosController : Controller
{
    private readonly ApplicationDbContext _context;

    public NotificacionesEventosController(ApplicationDbContext context)
    {
        _context = context;
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult ObtenerNotificacionesMenu()
    {
        var alertasInventario = _context.Inventario
            .Where(p => p.CantidadActual <= p.StockMinimo || p.CantidadActual >= p.StockMaximo)
            .Select(p => new {
                id = "inv_" + p.Id, // <-- NUEVO: Le damos un ID único al producto
                tipo = "inventario",
                texto = p.CantidadActual <= p.StockMinimo
                        ? $"¡Stock bajo! {p.Nombre} tiene solo {p.CantidadActual} pz."
                        : $"¡Stock alto! {p.Nombre} tiene {p.CantidadActual} pz.",
                enlace = "/Productos/Index"
            }).ToList();

        var avisosNuevos = _context.Eventos
            .OrderByDescending(e => e.Id)
            .Take(3)
            .Select(e => new {
                id = "aviso_" + e.Id, // <-- NUEVO: Le damos un ID único al aviso
                tipo = "aviso",
                texto = $"Nuevo aviso: {e.Titulo}",
                enlace = "/Eventos/Index"
            }).ToList();

        var todasLasNotificaciones = new List<dynamic>();
        todasLasNotificaciones.AddRange(alertasInventario);
        todasLasNotificaciones.AddRange(avisosNuevos);

        // Ya no mandamos el "total" desde aquí, lo calcularemos en el diseño
        return Json(new
        {
            mensajes = todasLasNotificaciones
        });
    }
    // 1. Mostrar la pantalla en blanco para crear
    public IActionResult Create(int eventoId)
    {
        // Le ponemos la "engrapadora" desde el inicio para que no se pierda la conexión
        var notificacion = new NotificacionEvento
        {
            EventoId = eventoId
        };

        return View(notificacion);
    }

    // 2. Guardar el formato nuevo de alimentos en la base de datos
    [HttpPost]
    public async Task<IActionResult> Create(NotificacionEvento notificacion)
    {
        if (ModelState.IsValid)
        {
            _context.NotificacionesEventos.Add(notificacion);
            await _context.SaveChangesAsync();

            // Cuando termine, lo regresamos al tablero principal
            return RedirectToAction("Index", "Eventos");
        }
        return View(notificacion);
    }

    // ==========================================
    // 3. PIEZA NUEVA: GUARDAR CAMBIOS AL EDITAR (PESTAÑA 2)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> Edit(NotificacionEvento notificacion)
    {
        // ESTA ES LA LÍNEA MÁGICA QUE ARREGLA EL GUARDADO DEL SEGUNDO FORMATO
        ModelState.Remove("Evento");

        if (ModelState.IsValid)
        {
            _context.Update(notificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Eventos", new { id = notificacion.EventoId });
        }
        return RedirectToAction("Index", "Eventos");
    }
}