using System;
using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class Evento
    {
        // ==========================================
        // CAMPOS ORIGINALES DEL AVISO
        // ==========================================
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? DetallesLogistica { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool EsUrgente { get; set; }
        public string? Creador { get; set; }

        // ==========================================
        // NUEVOS CAMPOS: INSTRUCTIVO DE GRUPO
        // ==========================================
        // 1. Datos Generales
        public string? Folio { get; set; }

        [Display(Name = "Fecha")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Grupo")]
        public string? Grupo { get; set; }

        [Display(Name = "Agencia o Cía")]
        public string? Agencia { get; set; }

        [Display(Name = "Tipo de Grupo")]
        public string? TipoGrupo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Coordinador { get; set; }
        public string? Rfc { get; set; }
        public string? Ejecutivo { get; set; }

        // 2. Logística de Estancia
        [Display(Name = "Fecha de Entrada")]
        public DateTime? FechaEntrada { get; set; }

        [Display(Name = "Fecha de Salida")]
        public DateTime? FechaSalida { get; set; }

        [Display(Name = "Hora de Llegada")]
        public string? HoraLlegada { get; set; }

        [Display(Name = "No. de Reservación")]
        public string? NumeroReservacion { get; set; }

        // 3. Habitaciones (Cantidades)
        public int? Sencillas { get; set; }
        public int? Dobles { get; set; }
        public int? Triples { get; set; }
        public int? Cuadruples { get; set; }
        public int? CortesiasHabitacion { get; set; }

        // 4. Finanzas y Cobros
        public decimal? CostoHospedaje { get; set; }
        public decimal? CostoAlimentos { get; set; }
        public decimal? CostoPropinas { get; set; }
        public decimal? TotalEvento { get; set; }
        public decimal? Anticipo { get; set; }

        public bool CuentaMaestra { get; set; }
        public bool CuentasIndividuales { get; set; }

        // 5. Facturación
        [Display(Name = "Facturar a")]
        public string? FacturarA { get; set; }
        [Display(Name = "Domicilio Facturación")]
        public string? DomicilioFacturacion { get; set; }
        [Display(Name = "RFC Facturación")]
        public string? RfcFacturacion { get; set; }
        [Display(Name = "Teléfono Facturación")]
        public string? TelefonoFacturacion { get; set; }

        // 6. Operación y Departamentos
        public string? Guia { get; set; }
        public string? Despertador { get; set; }
        public string? Equipaje { get; set; }
        public string? NotasAlimentos { get; set; }
        public string? CortesiasExtras { get; set; }
        public string? Observaciones { get; set; }
    }
}