using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Services
{
    // Heredamos de BackgroundService para que funcione como un proceso fantasma
    public class LimpiadorAvisosService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _env;

        public LimpiadorAvisosService(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _env = env;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Este ciclo se ejecuta infinitamente mientras el servidor esté encendido
            while (!stoppingToken.IsCancellationRequested)
            {
                await LimpiarArchivosYBaseDeDatos();

                // Aquí le decimos al robot que se duerma y vuelva a revisar en 24 horas.
                // (TRUCO: Si quieres probarlo ahorita para ver si funciona, comentalo y pon: 
                // await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); para que lo haga cada minuto).
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task LimpiarArchivosYBaseDeDatos()
        {
            // Como el servicio corre siempre, tenemos que "abrir y cerrar" la conexión a la base de datos cada vez
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Calculamos la fecha de hace 6 meses exactos
                var fechaLimite = DateTime.Now.AddMonths(-6);

                // Buscamos los avisos que sean más viejos que esa fecha
                var eventosViejos = await context.Eventos
                                                 .Where(e => e.FechaCreacion < fechaLimite)
                                                 .ToListAsync();

                if (eventosViejos.Any())
                {
                    foreach (var evento in eventosViejos)
                    {
                        // 1. EXTRAER LA LISTA DE ARCHIVOS (Igual que en el index)
                        string archivosAdjuntos = "";

                        if (!string.IsNullOrEmpty(evento.DetallesLogistica))
                        {
                            if (evento.DetallesLogistica.Contains("===ARCHIVOS==="))
                            {
                                var partes = evento.DetallesLogistica.Split(new[] { "===ARCHIVOS===" }, StringSplitOptions.None);
                                if (partes.Length > 1) { archivosAdjuntos = partes[1]; }
                            }
                            else if (evento.DetallesLogistica.Contains("::"))
                            {
                                archivosAdjuntos = evento.DetallesLogistica;
                            }
                        }

                        // 2. BORRAR LOS ARCHIVOS FÍSICOS DEL DISCO DURO
                        if (!string.IsNullOrEmpty(archivosAdjuntos))
                        {
                            var archivos = archivosAdjuntos.Split('|');
                            foreach (var arch in archivos)
                            {
                                if (arch.Contains("::"))
                                {
                                    var datosArchivo = arch.Split("::");
                                    if (datosArchivo.Length == 2)
                                    {
                                        // Extraemos el nombre único del archivo
                                        string rutaInterna = datosArchivo[1]; // ej: "/uploads/1234-abcd.pdf"
                                        string nombreArchivo = Path.GetFileName(rutaInterna);

                                        // Buscamos la ruta real en tu computadora/servidor
                                        string rutaFisica = Path.Combine(_env.WebRootPath, "uploads", nombreArchivo);

                                        // Si el archivo existe de verdad, ¡lo aniquilamos!
                                        if (File.Exists(rutaFisica))
                                        {
                                            File.Delete(rutaFisica);
                                        }
                                    }
                                }
                            }
                        }

                        // 3. BORRAR EL AVISO DE LA BASE DE DATOS
                        context.Eventos.Remove(evento);
                    }

                    // Guardamos los cambios finales en la base de datos
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}