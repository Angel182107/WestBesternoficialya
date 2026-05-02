using WestBesternoficialya.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Aquí registramos las tablas que definimos antes
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<AcuseRecibo> AcusesRecibo { get; set; }
    public DbSet<Habitacion> Habitaciones { get; set; }
    public DbSet<Incidencia> Incidencias { get; set; }
    public DbSet<Producto> Inventario { get; set; }
}