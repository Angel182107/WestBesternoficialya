namespace WestBesternoficialya.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string DetallesLogistica { get; set; } // Aquí va lo de Box Lunch, alimentos, etc.
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool EsUrgente { get; set; }
    }
}
