using Microsoft.AspNetCore.SignalR;
using VeterinariaMvc.Models.Chats;
using VeterinariaMvc.Services.Chats;

namespace VeterinariaMvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task RegistrarUsuario(int idUsuario)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{idUsuario}");
        }

        public async Task UnirseAConversacion(int idConversacion)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conv-{idConversacion}");
        }

   
        public async Task EnviarMensaje(int idConversacion, int idRemitente, string texto)
        {
            var (mensaje, idUsuarioDestino) =
                await _chatService.EnviarMensajeAsync(idConversacion, idRemitente, texto);

            // 1. Mensaje al área de chat (solo quienes están viendo la conversación)
            await Clients.Group($"conv-{idConversacion}").SendAsync("RecibirMensaje", new
            {
                mensaje.Id,
                mensaje.IdConversacion,
                mensaje.IdRemitente,
                Mensaje = mensaje.Mensaje,
                FechaEnvio = mensaje.FechaEnvio.ToString("HH:mm"),
                mensaje.Leido
            });

            // 2. Actualizar sidebar de ambos usuarios
            var sidebarData = new
            {
                IdConversacion = idConversacion,
                UltimoMensaje = mensaje.Mensaje,
                FechaEnvio = mensaje.FechaEnvio.ToString("HH:mm"),
                IdRemitente = idRemitente
            };

            await Clients.Group($"user-{idRemitente}")
                .SendAsync("ActualizarSidebar", sidebarData);
            await Clients.Group($"user-{idUsuarioDestino}")
                .SendAsync("ActualizarSidebar", sidebarData);
        }

        public async Task MarcarLeidos(int idConversacion, int idUsuarioReceptor)
        {
            await _chatService.MarcarComoLeidosAsync(idConversacion, idUsuarioReceptor);
            await Clients.Group($"conv-{idConversacion}").SendAsync("MensajesLeidos", idConversacion);
        }
    }
}
