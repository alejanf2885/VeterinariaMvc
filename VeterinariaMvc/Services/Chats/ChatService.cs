using VeterinariaMvc.Models.Chats;
using VeterinariaMvc.Repositories.Chats;

namespace VeterinariaMvc.Services.Chats
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;

        public ChatService(IChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }

        public async Task<ChatConversacion> ObtenerOCrearConversacionAsync(int idCliente, int idVeterinario)
        {
            return await _chatRepo.ObtenerOCrearConversacionAsync(idCliente, idVeterinario);
        }

        public async Task<(ChatMensaje Mensaje, int IdUsuarioDestino)>
            EnviarMensajeAsync(int idConversacion, int idRemitente, string texto)
        {
            ChatMensaje mensaje = await _chatRepo.GuardarMensajeAsync(
                idConversacion,
                idRemitente,
                texto
            );

            var (idUsuarioCliente, idUsuarioVeterinario) =
                await _chatRepo.ObtenerUsuariosConversacionAsync(idConversacion);

            int idUsuarioDestino = idRemitente == idUsuarioCliente
                ? idUsuarioVeterinario
                : idUsuarioCliente;

            return (mensaje, idUsuarioDestino);
        }

        public async Task<List<ChatMensaje>> ObtenerHistorialAsync(int idConversacion)
        {
            return await _chatRepo.ObtenerHistorialAsync(idConversacion);
        }

        public async Task MarcarComoLeidosAsync(int idConversacion, int idUsuarioReceptor)
        {
            await _chatRepo.MarcarComoLeidosAsync(idConversacion, idUsuarioReceptor);
        }

        public async Task<List<ChatConversacion>> ObtenerConversacionesPorUsuarioAsync(int idUsuario)
        {
            return await _chatRepo.ObtenerConversacionesPorUsuarioAsync(idUsuario);
        }

        public async Task<string> ObtenerNombreUsuarioAsync(int idUsuario)
        {
            return await _chatRepo.ObtenerNombreUsuarioAsync(idUsuario);
        }

        public async Task<string?> ObtenerImagenUsuarioAsync(int idUsuario)
        {
            return await _chatRepo.ObtenerImagenUsuarioAsync(idUsuario);
        }

        public async Task<int> ContarMensajesNoLeidosAsync(int idConversacion, int idUsuario)
        {
            return await _chatRepo.ContarMensajesNoLeidosAsync(idConversacion, idUsuario);
        }

        public async Task<ChatMensaje?> ObtenerUltimoMensajeAsync(int idConversacion)
        {
            return await _chatRepo.ObtenerUltimoMensajeAsync(idConversacion);
        }

        public async Task<(int IdUsuarioCliente, int IdUsuarioVeterinario)> ObtenerUsuariosConversacionAsync(int idConversacion)
        {
            return await _chatRepo.ObtenerUsuariosConversacionAsync(idConversacion);
        }

        public async Task<List<VeterinarioDisponibleViewModel>> ObtenerVeterinariosDisponiblesAsync(int idUsuario)
        {
            return await _chatRepo.ObtenerVeterinariosDisponiblesAsync(idUsuario);
        }

        public async Task<int?> ObtenerIdClientePorUsuarioAsync(int idUsuario)
        {
            return await _chatRepo.ObtenerIdClientePorUsuarioAsync(idUsuario);
        }
    }
}