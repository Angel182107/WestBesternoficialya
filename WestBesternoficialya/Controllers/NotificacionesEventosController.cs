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
    [AllowAnonymous] // Permite que cualquier empleado vea la campanita, aunque no sea Administrador
    [HttpGet]
    public IActionResult ObtenerContador()
    {
        // Aquí le decimos a la base de datos que cuente cuántos eventos/avisos existen.
        int cantidadAvisosNuevos = _context.Eventos.Count();

        return Json(cantidadAvisosNuevos);
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