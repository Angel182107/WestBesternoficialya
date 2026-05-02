namespace WestBesternoficialya.Models
{
    public class Incidencia
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } // "Foco roto"
        public DateTime FechaReporte { get; set; }
        public bool EstaResuelto { get; set; }
        public int HabitacionId { get; set; }
        public int UsuarioReportaId { get; set; } // Ama de llaves

        public Habitacion Habitacion { get; set; }
    }
}
