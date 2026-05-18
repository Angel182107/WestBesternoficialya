using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WestBesternoficialya.Controllers;

[Route("Acceso")]
public class AccesoController : Controller
{
    [Route("Index")]
    [Route("")]
    public IActionResult Index()
    {
        return View("~/Views/Acceso/Index.cshtml");
    }

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

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "El usuario o la contraseña son incorrectos.";
        return View("~/Views/Acceso/Index.cshtml");
    }

    [Route("Salir")]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Acceso");
    }
}