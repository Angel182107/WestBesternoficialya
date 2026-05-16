using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- Asegúrate de tener esta línea arriba para el ViewBag
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

    public async Task<IActionResult> Index()
    {
        var productos = await _context.Inventario.ToListAsync();
        return View(productos);
    }

    // --- EL VIAJE DE IDA (GET): Formulario en blanco ---
    public IActionResult Create()
    {
        // Llevamos la charola con los departamentos
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre");
        return View();
    }

    // --- EL VIAJE DE VUELTA (POST): Guardar el producto ---
    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        // ¡Magia! Le decimos al inspector que se relaje y no pida el objeto Departamento completo
        ModelState.Remove("Departamento");

        if (ModelState.IsValid)
        {
            _context.Inventario.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si hay un error, recargamos la charola para que el menú no se rompa
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
        return View(producto);
    }
}