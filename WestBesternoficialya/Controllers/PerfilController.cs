using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using WestBesternoficialya.Models.ViewModels;
using System.Threading.Tasks;

namespace WestBesternoficialya.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfilController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- PANTALLA PRINCIPAL DEL PERFIL (GET) ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var nombreUsuario = User.Identity?.Name;
            var usuario = await _context.Usuarios
                                        .Include(u => u.Departamento)
                                        .FirstOrDefaultAsync(u => u.NombreCompleto == nombreUsuario || u.Email == nombreUsuario);

            if (usuario == null) return NotFound();

            var model = new PerfilViewModel
            {
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Departamento = usuario.Departamento?.Nombre ?? "Sin Asignar"
            };

            return View(model);
        }

        // --- PROCESAR CAMBIO DE CONTRASEÑA (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(PerfilViewModel model)
        {
            var nombreUsuario = User.Identity?.Name;
            var usuario = await _context.Usuarios
                                        .Include(u => u.Departamento)
                                        .FirstOrDefaultAsync(u => u.NombreCompleto == nombreUsuario || u.Email == nombreUsuario);

            if (usuario == null) return NotFound();

            // MAGIA AQUÍ: Le decimos al sistema que ignore los campos que no viajan en el formulario
            ModelState.Remove("NombreCompleto");
            ModelState.Remove("Email");
            ModelState.Remove("Departamento");

            // Ahora sí, validamos lo que importa
            if (ModelState.IsValid)
            {
                // 1. Verificamos que la contraseña actual que escribió coincida con la de la BD
                if (usuario.Password != model.PasswordActual)
                {
                    // Si está mal, disparamos el error rojo en la pantalla
                    ModelState.AddModelError("PasswordActual", "La contraseña actual no es correcta.");
                }
                else
                {
                    // 2. ¡Todo bien! Actualizamos la base de datos
                    usuario.Password = model.PasswordNueva;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["MensajeExito"] = "Contraseña actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Si llegamos a este punto, significa que hubo un error (contraseña equivocada o no coinciden).
            // Tenemos que volver a llenar los datos de la credencial para que no desaparezcan al recargar la vista.
            model.NombreCompleto = usuario.NombreCompleto;
            model.Email = usuario.Email;
            model.Departamento = usuario.Departamento?.Nombre ?? "Sin Asignar";

            return View("Index", model);
        }
    }
}