using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using WestBesternoficialya.Data;

namespace WestBesternoficialya.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MantenimientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Mostrar la tabla de materiales
        public async Task<IActionResult> Index()
        {
            var materiales = await _context.MaterialesMantenimiento
                .OrderByDescending(m => m.Existencia <= m.Minimo)
                .ThenBy(m => m.Marca)
                .ToListAsync();

            return View(materiales);
        }

        // 2. Mostrar el formulario para registrar material
        public IActionResult Create()
        {
            return View(new MaterialMantenimiento { Fecha = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialMantenimiento modelo)
        {
            if (ModelState.IsValid)
            {
                // NUEVA LÓGICA: Compramos lo necesario para alcanzar el Mínimo
                modelo.Comprar = modelo.Minimo - modelo.Existencia;

                // Si la existencia ya es mayor al mínimo, no compramos nada (0)
                if (modelo.Comprar < 0)
                {
                    modelo.Comprar = 0;
                }

                _context.MaterialesMantenimiento.Add(modelo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(modelo);
        }

        // 3. VIAJE DE IDA: Mostrar la pantalla con los datos actuales para editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.MaterialesMantenimiento.FindAsync(id);
            if (material == null) return NotFound();

            return View(material);
        }

        // 4. VIAJE DE VUELTA: Guardar la nueva cantidad (Edit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialMantenimiento modelo)
        {
            if (id != modelo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // NUEVA LÓGICA: Recalculamos basándonos en el Mínimo
                modelo.Comprar = modelo.Minimo - modelo.Existencia;

                if (modelo.Comprar < 0)
                {
                    modelo.Comprar = 0;
                }

                _context.Update(modelo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(modelo);
        }
    }
}