using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Controllers;

public class DepartamentosController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepartamentosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var departamentos = await _context.Departamentos.ToListAsync();

        return View(departamentos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Departamento departamento)
    {
        // Verificamos que los datos que escribió el usuario sean correctos
        if (ModelState.IsValid)
        {
            // Le damos el papel lleno a la base de datos para que lo prepare
            _context.Departamentos.Add(departamento);

            // Guardamos los cambios en MySQL (¡AQUÍ SE GUARDA!)
            await _context.SaveChangesAsync();

            // Regresamos al usuario a la tabla principal para que vea su nuevo registro
            return RedirectToAction(nameof(Index));
        }

        // Si dejó el nombre en blanco, le regresamos el formulario para que lo corrija
        return View(departamento);
    }
    // ==========================================
    // SECCIÓN: EDITAR (ACTUALIZAR)
    // ==========================================

    // 1. VIAJE DE IDA (GET): Busca el departamento en la BD y lo muestra en el formulario
    public async Task<IActionResult> Edit(int? id)
    {
        // Si no nos pasan un ID, marcamos error
        if (id == null) return NotFound();

        // El mesero busca en la cocina el departamento con ese número exacto
        var departamento = await _context.Departamentos.FindAsync(id);

        if (departamento == null) return NotFound();

        // Le entrega a la pantalla el "papel" ya lleno con los datos actuales
        return View(departamento);
    }

    // 2. VIAJE DE VUELTA (POST): Recibe los datos corregidos y los guarda
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Departamento departamento)
    {
        if (id != departamento.Id) return NotFound();

        if (ModelState.IsValid)
        {
            // Le dice a la base de datos que este registro fue modificado
            _context.Update(departamento);
            await _context.SaveChangesAsync();

            // Lo regresa a la lista principal
            return RedirectToAction(nameof(Index));
        }
        return View(departamento);
    }

    // ==========================================
    // SECCIÓN: ELIMINAR (BORRAR)
    // ==========================================

    // 1. VIAJE DE IDA (GET): Muestra una pantalla preguntando "¿Estás seguro?"
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var departamento = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);

        if (departamento == null) return NotFound();

        return View(departamento);
    }

    // 2. VIAJE DE VUELTA (POST): El usuario dijo "Sí, bórralo", así que lo destruimos
    [HttpPost, ActionName("Delete")] // ActionName sirve para que la URL siga diciendo /Delete
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento != null)
        {
            // Ordenamos eliminar el papel
            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync(); // Confirmamos el cambio en MySQL
        }

        return RedirectToAction(nameof(Index));
    }
}