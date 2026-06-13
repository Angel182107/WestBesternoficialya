using System;
using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class Memorandum
    {
        public int Id { get; set; }

        // ==========================================
        // LA ENGRAPADORA (Relación con el Aviso principal)
        // ==========================================
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        // ==========================================
        // ENCABEZADO
        // ==========================================
        public string? Para { get; set; }
        public string? De { get; set; }
        public string? FechaMemorandum { get; set; }
        public string? Asunto { get; set; }

        // ==========================================
        // CUERPO GENERAL Y FECHAS
        // ==========================================
        public string? Saludo { get; set; } // Ej: "POR MEDIO DE LA PRESENTE..."
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }
        public string? Garantia { get; set; }
        public string? NombreHuesped { get; set; }

        // ==========================================
        // TABLA DE COSTOS (Filas dinámicas)
        // ==========================================
        public string? Concepto1 { get; set; }
        public string? Cantidad1 { get; set; }
        public string? Noches1 { get; set; }
        public decimal? Costo1 { get; set; }
        public decimal? Total1 { get; set; }

        public string? Concepto2 { get; set; }
        public string? Cantidad2 { get; set; }
        public string? Noches2 { get; set; }
        public decimal? Costo2 { get; set; }
        public decimal? Total2 { get; set; }

        public string? Concepto3 { get; set; }
        public string? Cantidad3 { get; set; }
        public string? Noches3 { get; set; }
        public decimal? Costo3 { get; set; }
        public decimal? Total3 { get; set; }

        public string? Concepto4 { get; set; }
        public string? Cantidad4 { get; set; }
        public string? Noches4 { get; set; }
        public decimal? Costo4 { get; set; }
        public decimal? Total4 { get; set; }

        public decimal? GranTotal { get; set; }

        // ==========================================
        // INSTRUCCIONES POR DEPARTAMENTO
        // ==========================================
        public string? InstruccionesRecepcion { get; set; }
        public string? InstruccionesRestaurante { get; set; }
        public string? InstruccionesAmaLlaves { get; set; }

        // ==========================================
        // NOTA FINAL
        // ==========================================
        public string? NotaFinal { get; set; } // Ej: "CUALQUIER SERVICIO EXTRA..."
    }
}