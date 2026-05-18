using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization; // <-- No olvides esta línea hasta arriba de todo el archivo

namespace WestBesternoficialya.Controllers;

[Authorize] // <-- ¡ESTE ES EL CANDADO! Nadie entra sin gafete.
public class ReservacionesController : Controller
{
    // ... el resto de tu código ...
    private readonly ApplicationDbContext _context;

    public ReservacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- EL LIBRO DE REGISTROS (Pantalla principal) ---
    public async Task<IActionResult> Index()
    {
        // Traemos todas las reservaciones e incluimos los datos del cuarto
        var reservaciones = await _context.Reservaciones.Include(r => r.Habitacion).ToListAsync();
        return View(reservaciones);
    }

    // --- VIAJE DE IDA: Formulario para el nuevo cliente ---
    public IActionResult Create()
    {
        // TRUCO 1: Solo mostramos en la lista las habitaciones que están "Limpias"
        var cuartosDisponibles = _context.Habitaciones.Where(h => h.Estado == "Limpia").ToList();

        ViewBag.Habitaciones = new SelectList(cuartosDisponibles, "Id", "Numero");
        return View();
    }

    // --- VIAJE DE VUELTA: Guardar, calcular y actualizar ---
    [HttpPost]
    public async Task<IActionResult> Create(Reservacion reservacion)
    {
        // Calmamos al inspector para que no pida estos datos en el formulario
        ModelState.Remove("Habitacion");

        if (ModelState.IsValid)
        {
            // Buscamos el cuarto real que el cliente acaba de elegir
            var cuarto = await _context.Habitaciones.FindAsync(reservacion.HabitacionId);

            // TRUCO 2: Calculadora Automática
            // Contamos los días entre la entrada y la salida
            int noches = (reservacion.FechaSalida - reservacion.FechaEntrada).Days;

            // Si por error ponen el mismo día, cobramos mínimo 1 noche
            if (noches <= 0) { noches = 1; }

            // Multiplicamos las noches por el precio de ese cuarto en específico
            reservacion.TotalCobrar = noches * cuarto.Precio;

            // TRUCO 3: Sincronización
            // Le cambiamos el letrero al cuarto para que ya no se pueda volver a rentar
            cuarto.Estado = "Ocupada";
            _context.Update(cuarto);

            // Guardamos la reservación en la caja fuerte
            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Si hay un error, recargamos la lista de cuartos limpios
        var cuartosDisponibles = _context.Habitaciones.Where(h => h.Estado == "Limpia").ToList();
        ViewBag.Habitaciones = new SelectList(cuartosDisponibles, "Id", "Numero", reservacion.HabitacionId);
        return View(reservacion);
    }
    // ==========================================
    // SECCIÓN: CHECK-OUT (ENTREGA DE LLAVES)
    // ==========================================
    public async Task<IActionResult> CheckOut(int id)
    {
        // 1. Buscamos la reservación y los datos de su cuarto
        var reservacion = await _context.Reservaciones
            .Include(r => r.Habitacion)
            .FirstOrDefaultAsync(r => r.Id == id);

        // Si la encontramos y el cliente sigue activo...
        if (reservacion != null && reservacion.EstadoReserva == "Activa")
        {
            // 2. Marcamos la cuenta como Terminada
            reservacion.EstadoReserva = "Terminada";

            // 3. ¡Magia! Le cambiamos el letrero al cuarto para que limpieza sepa que ya pueden entrar
            if (reservacion.Habitacion != null)
            {
                reservacion.Habitacion.Estado = "Sucia";
                _context.Update(reservacion.Habitacion);
            }

            // Guardamos todos los cambios de golpe
            _context.Update(reservacion);
            await _context.SaveChangesAsync();
        }

        // Regresamos al libro de registros
        return RedirectToAction(nameof(Index));
    }
    // ==========================================
    // SECCIÓN: ELIMINAR DEL HISTORIAL
    // ==========================================
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // Buscamos la reservación y los datos de su cuarto para mostrarlos antes de borrar
        var reservacion = await _context.Reservaciones
            .Include(r => r.Habitacion)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (reservacion == null) return NotFound();

        return View(reservacion);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reservacion = await _context.Reservaciones.FindAsync(id);
        if (reservacion != null)
        {
            _context.Reservaciones.Remove(reservacion);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    // ==========================================
    // SECCIÓN: EDITAR RESERVACIÓN
    // ==========================================
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var reservacion = await _context.Reservaciones.FindAsync(id);
        if (reservacion == null) return NotFound();

        // Llevamos la charola con los cuartos. 
        // TRUCO: Mostramos los cuartos limpios PLUS el cuarto que ya tiene asignado actualmente
        var cuartosDisponibles = _context.Habitaciones
            .Where(h => h.Estado == "Limpia" || h.Id == reservacion.HabitacionId)
            .ToList();

        ViewBag.Habitaciones = new SelectList(cuartosDisponibles, "Id", "Numero", reservacion.HabitacionId);

        return View(reservacion);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Reservacion reservacion)
    {
        if (id != reservacion.Id) return NotFound();

        ModelState.Remove("Habitacion");

        if (ModelState.IsValid)
        {
            // Calculadora automática (por si le cambiaron los días de estancia o el cuarto)
            var cuarto = await _context.Habitaciones.FindAsync(reservacion.HabitacionId);
            int noches = (reservacion.FechaSalida - reservacion.FechaEntrada).Days;
            if (noches <= 0) { noches = 1; }
            reservacion.TotalCobrar = noches * cuarto.Precio;

            // Guardamos la corrección
            _context.Update(reservacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si hay error, recargamos la lista
        var cuartosDisponibles = _context.Habitaciones
            .Where(h => h.Estado == "Limpia" || h.Id == reservacion.HabitacionId)
            .ToList();
        ViewBag.Habitaciones = new SelectList(cuartosDisponibles, "Id", "Numero", reservacion.HabitacionId);

        return View(reservacion);
    }
}