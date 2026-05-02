namespace WestBesternoficialya.Models
{
    public class AcuseRecibo
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaHoraFirma { get; set; } // Esta es tu "Firma Digital"

        public Evento Evento { get; set; }
        public Usuario Usuario { get; set; }
    }
}
