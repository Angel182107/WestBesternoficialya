using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Controllers;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- LA LISTA DEL PERSONAL ---
    public async Task<IActionResult> Index()
    {
        // OJO AQUÍ: Usamos .Include() 
        // Esto le dice al mesero: "Trae al usuario, pero también jálate los datos de su departamento". 
        // Si no ponemos Include, solo veríamos un ID (ej. Departamento 1) en vez de "Administración".
        var usuarios = await _context.Usuarios.Include(u => u.Departamento).ToListAsync();
        return View(usuarios);
    }

    // --- EL VIAJE DE IDA (GET): Formulario en blanco ---
    public IActionResult Create()
    {
        // ViewBag es como una "charola extra" que lleva el mesero a la mesa.
        // Aquí subimos a la charola todos los departamentos de la cocina para armar el menú desplegable.
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre");

        return View();
    }

    // --- EL VIAJE DE VUELTA (POST): Guardar al empleado ---
    [HttpPost]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        ModelState.Remove("Departamento");
        if (ModelState.IsValid)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si se equivocó y falta un dato, le volvemos a cargar la lista de departamentos para que no marque error
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", usuario.DepartamentoId);
        return View(usuario);
    }
    // ==========================================
    // SECCIÓN: EDITAR (ACTUALIZAR)
    // ==========================================

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        // Llevamos la charola con los departamentos y dejamos seleccionado el que ya tenía el usuario
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", usuario.DepartamentoId);
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.Id) return NotFound();

        // Relajamos al inspector igual que en el Create
        ModelState.Remove("Departamento");

        if (ModelState.IsValid)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si hay error, volvemos a cargar la charola
        ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id", "Nombre", usuario.DepartamentoId);
        return View(usuario);
    }

    // ==========================================
    // SECCIÓN: ELIMINAR (BORRAR)
    // ==========================================

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // Usamos Include para poder mostrar el nombre del departamento en la pantalla de confirmación
        var usuario = await _context.Usuarios
            .Include(u => u.Departamento)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (usuario == null) return NotFound();

        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}