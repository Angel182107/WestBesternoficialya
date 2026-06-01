using System;
using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class FormatoReservacion
    {
        public int Id { get; set; }

        // ==========================================
        // LA ENGRAPADORA (Conexión con el Instructivo Evento)
        // ==========================================
        // Esto le dice a MySQL: "Este formato financiero le pertenece a X evento"
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        // ==========================================
        // 1. ENCABEZADO DE RESERVACIÓN
        // ==========================================
        public string? Folio { get; set; }

        [Display(Name = "Fecha Elaboración")]
        public DateTime? FechaElaboracion { get; set; }
        public string? NombreGrupo { get; set; }
        public string? PersonaQueSolicita { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }

        // ==========================================
        // 2. DETALLES DE RESERVACIÓN
        // ==========================================
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? TotalNoches { get; set; }
        public int? TotalHabitaciones { get; set; }

        // ==========================================
        // 3. DESGLOSE DE HABITACIONES
        // ==========================================
        public int? CantidadSencilla { get; set; }
        public decimal? TarifaSencilla { get; set; }
        public decimal? TotalSencilla { get; set; }

        public int? CantidadDoble { get; set; }
        public decimal? TarifaDoble { get; set; }
        public decimal? TotalDoble { get; set; }

        public int? CantidadTriple { get; set; }
        public decimal? TarifaTriple { get; set; }
        public decimal? TotalTriple { get; set; }

        public int? CantidadCuadruple { get; set; }
        public decimal? TarifaCuadruple { get; set; }
        public decimal? TotalCuadruple { get; set; }

        // ==========================================
        // 4. TOTALES FINANCIEROS Y ANTICIPOS
        // ==========================================
        public decimal? Subtotal { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Ish { get; set; }
        public decimal? Total { get; set; }

        public decimal? AnticipoRequerido { get; set; }
        public DateTime? FechaLimiteAnticipo { get; set; }

        // ==========================================
        // 5. POLÍTICAS Y OBSERVACIONES
        // ==========================================
        public string? PoliticasCancelacion { get; set; }
        public string? Observaciones { get; set; }
    }
}