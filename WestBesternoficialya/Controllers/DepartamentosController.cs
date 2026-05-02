using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Asegúrate de que el nombre coincida con tu proyecto actual
using WestBesternoficialya.Data;
using WestBesternoficialya.Models;

namespace HotelSanPedroWeb.Controllers;

public class DepartamentosController : Controller
{
    // Aquí guardamos la conexión a la base de datos
    private readonly ApplicationDbContext _context;

    // Cuando el Controlador "nace", le entregamos las llaves de la base de datos
    public DepartamentosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Esta es la acción principal (la página que carga por defecto)
    public async Task<IActionResult> Index()
    {
        // El mesero va a la cocina (BD), toma todos los departamentos y los guarda en una lista
        var departamentos = await _context.Departamentos.ToListAsync();

        // El mesero le entrega esa lista a la Vista (Pantalla)
        return View(departamentos);
    }
}