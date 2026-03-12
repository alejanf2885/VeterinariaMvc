using VeterinariaMvc.Models.Chats;

namespace VeterinariaMvc.Models.Chats
{
    public class ChatPageViewModel
    {
        public int IdUsuarioActual { get; set; }
        public string NombreUsuarioActual { get; set; }
        public List<ChatConversacionItemViewModel> Conversaciones { get; set; } = new();
        public int? IdConversacionActiva { get; set; }
        public List<ChatMensaje>? HistorialActivo { get; set; }
        public string? NombreDestinatario { get; set; }
        public string? ImagenDestinatario { get; set; }
    }
}
