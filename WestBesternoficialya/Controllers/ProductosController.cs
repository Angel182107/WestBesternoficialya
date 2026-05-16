using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Controllers;

public class ProductosController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- PANTALLA PRINCIPAL ---
    public async Task<IActionResult> Index()
    {
        // .Include hace que podamos ver el nombre del departamento en la tabla
        var productos = await _context.Inventario.Include(p => p.Departamento).ToListAsync();
        return View(productos);
    }

    // --- CREAR ---
    public IActionResult Create()
    {
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        ModelState.Remove("Departamento");

        if (ModelState.IsValid)
        {
            _context.Inventario.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
        return View(producto);
    }

    // ==========================================
    // NUEVA SECCIÓN: EDITAR (ACTUALIZAR)
    // ==========================================
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var producto = await _context.Inventario.FindAsync(id);
        if (producto == null) return NotFound();

        // Llevamos los departamentos en la charola para el menú desplegable
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
        return View(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.Id) return NotFound();

        // Volvemos a calmar al inspector
        ModelState.Remove("Departamento");

        if (ModelState.IsValid)
        {
            _context.Update(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
        return View(producto);
    }

    // ==========================================
    // NUEVA SECCIÓN: ELIMINAR (BORRAR)
    // ==========================================
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // Traemos el producto y su departamento para mostrarlo antes de borrar
        var producto = await _context.Inventario
            .Include(p => p.Departamento)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (producto == null) return NotFound();

        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Inventario.FindAsync(id);
        if (producto != null)
        {
            _context.Inventario.Remove(producto);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}