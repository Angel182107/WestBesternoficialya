using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WestBesternoficialya.Controllers;

// GPS 1: Le decimos que esta clase entera responde a la palabra "Acceso"
[Route("Acceso")]
public class AccesoController : Controller
{
    // GPS 2: Le decimos que esta pantalla responde a la palabra "Index"
    [Route("Index")]
    [Route("")] // Por si acaso alguien escribe solo "/Acceso"
    public IActionResult Index()
    {
        return View("~/Views/Acceso/Index.cshtml");
    }

    // GPS 3: Ruta forzada para el botón de entrar
    [HttpPost("Entrar")]
    public async Task<IActionResult> Entrar(string correo, string clave)
    {
        if (correo == "admin" && clave == "12345")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Administrador")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Si entra, lo mandamos al inicio
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "El usuario o la contraseña son incorrectos.";
        return View("~/Views/Acceso/Index.cshtml");
    }

    // GPS 4: Ruta para cerrar sesión
    [Route("Salir")]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Acceso");
    }
}