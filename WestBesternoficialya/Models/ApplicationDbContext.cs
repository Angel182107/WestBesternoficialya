using WestBesternoficialya.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // ==========================================
    // TABLAS PRINCIPALES DEL ERP (LIMPIAS)
    // ==========================================
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<AcuseRecibo> AcusesRecibo { get; set; }
    public DbSet<Producto> Inventario { get; set; }
    public DbSet<MaterialMantenimiento> MaterialesMantenimiento { get; set; }
}