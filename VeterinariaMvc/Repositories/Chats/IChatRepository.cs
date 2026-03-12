using VeterinariaMvc.Models.Chats;

namespace VeterinariaMvc.Repositories.Chats
{
    public interface IChatRepository
    {
        Task<ChatConversacion> ObtenerOCrearConversacionAsync(int idCliente, int idVeterinario);
        Task<ChatMensaje> GuardarMensajeAsync(int idConversacion, int idRemitente, string texto);
        Task<List<ChatMensaje>> ObtenerHistorialAsync(int idConversacion);
        Task MarcarComoLeidosAsync(int idConversacion, int idUsuarioReceptor);
        Task<ChatConversacion> ObtenerConversacionPorIdAsync(int idConversacion);
        Task<(int IdUsuarioCliente, int IdUsuarioVeterinario)> ObtenerUsuariosConversacionAsync(int idConversacion);
        Task<List<ChatConversacion>> ObtenerConversacionesPorUsuarioAsync(int idUsuario);
        Task<string> ObtenerNombreUsuarioAsync(int idUsuario);
        Task<string?> ObtenerImagenUsuarioAsync(int idUsuario);
        Task<int> ContarMensajesNoLeidosAsync(int idConversacion, int idUsuario);
        Task<ChatMensaje?> ObtenerUltimoMensajeAsync(int idConversacion);
        Task<List<VeterinarioDisponibleViewModel>> ObtenerVeterinariosDisponiblesAsync(int idUsuario);
        Task<int?> ObtenerIdClientePorUsuarioAsync(int idUsuario);
    }
}
