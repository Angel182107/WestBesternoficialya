using System;
using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class NotificacionEvento
    {
        public int Id { get; set; }

        // ==========================================
        // LA ENGRAPADORA (Para que sepa de qué Aviso es esto)
        // ==========================================
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        // ==========================================
        // DATOS GENERALES
        // ==========================================
        public string? Folio { get; set; }
        public string? FechaEvento { get; set; } // Ej: "02 Y 03 DE ABRIL"
        public string? ClienteEmpresa { get; set; }
        public string? Ejecutivo { get; set; }

        // ==========================================
        // LOGÍSTICA DEL SALÓN / COMIDA
        // ==========================================
        public string? ConceptoServicio { get; set; } // Ej: "DESAYUNO AMERICANO"
        public string? Horario { get; set; }
        public string? Lugar { get; set; }
        public string? Montaje { get; set; }

        // ==========================================
        // COSTOS (Ocultos para el personal operativo)
        // ==========================================
        public decimal? Precio { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Servicio { get; set; }
        public decimal? Subtotal { get; set; }
        public int? NoPax { get; set; }
        public int? NoDias { get; set; }
        public decimal? Total { get; set; }

        // ==========================================
        // EXTRAS
        // ==========================================
        public string? Cortesias { get; set; }
        public string? Pago { get; set; } // Ej: "CARGAR A RECEPCION"
        public string? Observaciones { get; set; }
        public int? NoPax2 { get; set; }
        public int? NoPax3 { get; set; }
        public int? NoPax4 { get; set; }
        public int? NoPax5 { get; set; }

        public int? NoDias2 { get; set; }
        public int? NoDias3 { get; set; }
        public int? NoDias4 { get; set; }
        public int? NoDias5 { get; set; }

        public string? Estacionamiento { get; set; }
        public int? Preparar { get; set; }
        public int? Garantia { get; set; }
        public string? Menus { get; set; }
        public string? NotasAdicionales { get; set; }
    }
}