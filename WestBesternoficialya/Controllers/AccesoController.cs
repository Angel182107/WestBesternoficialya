using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Controllers;

[Route("Acceso")]
public class AccesoController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccesoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("Index")]
    [Route("")]
    public IActionResult Index()
    {
        return View("~/Views/Acceso/Index.cshtml");
    }

    [HttpPost("Entrar")]
    public async Task<IActionResult> Entrar(string correo, string clave)
    {
        // 1. Buscamos al empleado en la base de datos conectando con su tabla de Departamento
        var empleado = await _context.Usuarios
                                     .Include(u => u.Departamento) // <--- Esta línea jala los datos del departamento asignado
                                     .FirstOrDefaultAsync(u => u.Email == correo && u.Password == clave);

        // 2. Si las credenciales son correctas y el empleado existe
        if (empleado != null)
        {
            // 3. Fabricamos el gafete virtual usando el Nombre de su Departamento como su Rol
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, empleado.NombreCompleto), // Guarda su nombre completo
                new Claim(ClaimTypes.Role, empleado.Departamento.Nombre) // <--- Su nivel de acceso ahora es el nombre de su departamento
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Lo dejamos pasar a la pantalla de inicio
            return RedirectToAction("Index", "Home");
        }

        // Si se equivoca en sus datos
        ViewBag.Error = "El correo o la contraseña son incorrectos.";
        return View("~/Views/Acceso/Index.cshtml");
    }

    [Route("Salir")]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Acceso");
    }
}