using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models;

public class InstructivoGrupo
{
    [Key]
    public int Id { get; set; }

    public string Folio { get; set; }
    public string NombreGrupo { get; set; }
    public string Agencia { get; set; }
    public string Coordinador { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime FechaSalida { get; set; }

    public int HabSencillas { get; set; }
    public int HabDobles { get; set; }
    public int Noches { get; set; }
    public string Observaciones { get; set; }

    // --- DATOS FINANCIEROS (Solo Administrador) ---
    public decimal TarifaSencilla { get; set; }
    public decimal TarifaDoble { get; set; }
    public decimal TotalHospedaje { get; set; }
    public decimal Anticipo { get; set; }
    public string FacturacionRFC { get; set; }
}