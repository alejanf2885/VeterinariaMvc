namespace VeterinariaMvc.Models.Chats
{
    public class ChatConversacionItemViewModel
    {
        public int IdConversacion { get; set; }
        public string NombreOtroUsuario { get; set; }
        public string? ImagenOtroUsuario { get; set; }
        public string? UltimoMensaje { get; set; }
        public DateTime? FechaUltimoMensaje { get; set; }
        public int MensajesNoLeidos { get; set; }
    }
}
