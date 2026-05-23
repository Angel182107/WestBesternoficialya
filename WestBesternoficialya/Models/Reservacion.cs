using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models
{
    public class Reservacion
    {
        public int Id { get; set; }

        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal TotalCobrar { get; set; }
        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; }
        public string EstadoReserva { get; set; } = "Activa";
    }
}