using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WestBesternoficialya.Controllers
{
    [Authorize] // Todo el controlador está protegido, solo usuarios logueados entran
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pagina = 1)
        {
            int cantidadPorPagina = 10;

            // Traemos la lista ordenada del más nuevo al más viejo
            var query = _context.Eventos.OrderByDescending(e => e.FechaCreacion);

            // Contamos el total para saber cuántas páginas habrá
            int totalRegistros = await query.CountAsync();

            // Brincamos los que no son de esta página y tomamos solo 10
            var eventos = await query.Skip((pagina - 1) * cantidadPorPagina)
                                     .Take(cantidadPorPagina)
                                     .ToListAsync();

            // Guardamos estos datos en el ViewBag para dibujar los botones en la pantalla
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            return View(eventos);
        }

        // --- PUBLICAR NUEVO AVISO (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            // Quitamos la restricción de roles para que TODOS puedan entrar a publicar
            return View();
        }

        // --- PUBLICAR NUEVO AVISO (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento, List<IFormFile> archivosSubidos)
        {
            if (ModelState.IsValid)
            {
                evento.Creador = User.Identity?.Name ?? "Administrador";

                // Guardamos el texto que haya escrito el usuario (si es que escribió algo)
                string textoUsuario = evento.DetallesLogistica ?? "";

                // 1. Verificamos si el usuario subió uno o más archivos
                if (archivosSubidos != null && archivosSubidos.Count > 0)
                {
                    List<string> listaDeArchivos = new List<string>();

                    foreach (var archivo in archivosSubidos)
                    {
                        if (archivo.Length > 0)
                        {
                            // Generamos un nombre único para que no choquen si se llaman igual
                            string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                            string rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", nombreUnico);

                            using (var stream = new FileStream(rutaFisica, FileMode.Create))
                            {
                                await archivo.CopyToAsync(stream);
                            }

                            // Guardamos el nombre original y la ruta interna, separados por "::"
                            listaDeArchivos.Add(archivo.FileName + "::/uploads/" + nombreUnico);
                        }
                    }

                    // Unimos toda la lista de archivos con el símbolo "|"
                    string archivosTexto = string.Join("|", listaDeArchivos);

                    // Juntamos el texto del usuario con los archivos usando un separador especial
                    if (!string.IsNullOrEmpty(textoUsuario))
                    {
                        evento.DetallesLogistica = textoUsuario + "===ARCHIVOS===" + archivosTexto;
                    }
                    else
                    {
                        evento.DetallesLogistica = "===ARCHIVOS===" + archivosTexto;
                    }
                }
                else
                {
                    // Si no subió archivos, DetallesLogistica se queda solo con el texto que ya traía
                    evento.DetallesLogistica = textoUsuario;
                }

                _context.Add(evento);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Aviso publicado correctamente.";

                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        // --- VER DETALLES ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null) return NotFound();
            return View(evento);
        }

        // --- EDITAR ---
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null) return NotFound();

            // Seguridad: Solo el creador o un Admin pueden ver la pantalla de edición
            if (evento.Creador != User.Identity?.Name && !User.IsInRole("Administrador"))
            {
                TempData["MensajeError"] = "No tienes permiso para editar este aviso.";
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            // Seguridad Extra en el POST
            var eventoOriginal = await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (eventoOriginal?.Creador != User.Identity?.Name && !User.IsInRole("Administrador"))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Update(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // --- ELIMINAR ---
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null) return NotFound();

            // Seguridad: Solo el creador o un Admin pueden ver la pantalla para borrar
            if (evento.Creador != User.Identity?.Name && !User.IsInRole("Administrador"))
            {
                TempData["MensajeError"] = "No tienes permiso para borrar este aviso.";
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                // Seguridad Final
                if (evento.Creador == User.Identity?.Name || User.IsInRole("Administrador"))
                {
                    _context.Eventos.Remove(evento);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // --- FIRMA DIGITAL ---
        public async Task<IActionResult> Firmar(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos.FindAsync(id);
            return View(evento);
        }

        public async Task<IActionResult> ReporteFirmas(int? id)
        {
            if (id == null) return NotFound();
            // Asegúrate de que las tablas de Acuses existan en tu DB si aún las usas
            var firmas = await _context.AcusesRecibo.Include(a => a.Usuario).Where(a => a.EventoId == id).ToListAsync();
            return View(firmas);
        }
    }
}