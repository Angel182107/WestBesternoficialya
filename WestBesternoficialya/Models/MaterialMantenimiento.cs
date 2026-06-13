using System;

namespace WestBesternoficialya.Models
{
    public class MaterialMantenimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Marca { get; set; }

        public int Maximo { get; set; }
        public int Minimo { get; set; }
        public int Existencia { get; set; }

        // Lo que falta para llegar al máximo (Se calculará solo)
        public int Comprar { get; set; }
    }
}