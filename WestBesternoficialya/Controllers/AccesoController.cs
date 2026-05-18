using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WestBesternoficialya.Controllers;

[Authorize]
public class AccessoController : Controller
{
    // ... el resto de tu código ...
    // --- LA PANTALLA DEL CANDADO ---
    public IActionResult Index()
    {
        return View();
    }

    // --- EL VIAJE DE VUELTA: Revisar la contraseña ---
    [HttpPost]
    public async Task<IActionResult> Entrar(string correo, string clave)
    {
        // Aquí probamos nuestra Llave Maestra temporal
        if (correo == "admin" && clave == "12345")
        {
            // Si es correcto, le creamos su gafete virtual
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Administrador")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Le colgamos el gafete e iniciamos sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Lo dejamos pasar al Centro de Mando
            return RedirectToAction("Index", "Home");
        }

        // Si se equivoca, le mandamos un mensaje de error rojo
        ViewBag.Error = "El usuario o la contraseña son incorrectos.";
        return View("Index");
    }

    // --- CERRAR SESIÓN ---
    public async Task<IActionResult> Salir()
    {
        // Le quitamos el gafete
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Acceso");
    }
}