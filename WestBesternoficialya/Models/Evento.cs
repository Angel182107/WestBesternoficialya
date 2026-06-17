using System;
using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string? DetallesLogistica { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string? Creador { get; set; }

    }
}