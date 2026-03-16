using VeterinariaMvc.Models.Chats;

namespace VeterinariaMvc.Services.Chats
{
    public interface IChatService
    {
        Task<ChatConversacion> ObtenerOCrearConversacionAsync(int idCliente, int idVeterinario);

        Task<(ChatMensaje Mensaje, int IdUsuarioDestino)> EnviarMensajeAsync(int idConversacion, int idRemitente, string texto);

        Task<List<ChatMensaje>> ObtenerHistorialAsync(int idConversacion);

        Task MarcarComoLeidosAsync(int idConversacion, int idUsuarioReceptor);

        Task<List<ChatConversacion>> ObtenerConversacionesPorUsuarioAsync(int idUsuario);
        Task<string> ObtenerNombreUsuarioAsync(int idUsuario);
        Task<string?> ObtenerImagenUsuarioAsync(int idUsuario);
        Task<int> ContarMensajesNoLeidosAsync(int idConversacion, int idUsuario);
        Task<ChatMensaje?> ObtenerUltimoMensajeAsync(int idConversacion);
        Task<(int IdUsuarioCliente, int IdUsuarioVeterinario)> ObtenerUsuariosConversacionAsync(int idConversacion);
        Task<List<VeterinarioDisponibleViewModel>> ObtenerVeterinariosDisponiblesAsync(int idUsuario);
        Task<int?> ObtenerIdClientePorUsuarioAsync(int idUsuario);

        Task<bool> EsParticipanteDeConversacionAsync(int idConversacion, int idUsuario);
    }
}
