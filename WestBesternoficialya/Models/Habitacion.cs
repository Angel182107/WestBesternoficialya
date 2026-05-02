namespace WestBesternoficialya.Models
{
    public class Habitacion
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Estado { get; set; } // "Disponible", "Sucia", "En Limpieza"
    }
}
