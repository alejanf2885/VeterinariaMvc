using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Hubs;
using VeterinariaMvc.Models.Chats;
using VeterinariaMvc.Services.Chats;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IEstadoUsuarioService _estadoUsuario;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(
            IChatService chatService,
            IEstadoUsuarioService estadoUsuario,
            IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _estadoUsuario = estadoUsuario;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(int? idConversacion)
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth");

            List<ChatConversacion> conversaciones =
                await _chatService.ObtenerConversacionesPorUsuarioAsync(usuario.Id);

            var listaConv = new List<ChatConversacionItemViewModel>();
            foreach (var conv in conversaciones)
            {
                var (idUsuarioCliente, idUsuarioVeterinario) =
                    await _chatService.ObtenerUsuariosConversacionAsync(conv.Id);

                int idOtroUsuario = usuario.Id == idUsuarioCliente
                    ? idUsuarioVeterinario
                    : idUsuarioCliente;

                string nombre = await _chatService.ObtenerNombreUsuarioAsync(idOtroUsuario);
                string? imagen = await _chatService.ObtenerImagenUsuarioAsync(idOtroUsuario);
                int noLeidos = await _chatService.ContarMensajesNoLeidosAsync(conv.Id, usuario.Id);
                ChatMensaje? ultimo = await _chatService.ObtenerUltimoMensajeAsync(conv.Id);

                listaConv.Add(new ChatConversacionItemViewModel
                {
                    IdConversacion = conv.Id,
                    NombreOtroUsuario = nombre,
                    ImagenOtroUsuario = imagen,
                    UltimoMensaje = ultimo?.Mensaje,
                    FechaUltimoMensaje = ultimo?.FechaEnvio,
                    MensajesNoLeidos = noLeidos
                });
            }

            List<ChatMensaje>? historial = null;
            string? nombreDestinatario = null;
            string? imagenDestinatario = null;

            if (idConversacion.HasValue && conversaciones.Any(c => c.Id == idConversacion.Value))
            {
                historial = await _chatService.ObtenerHistorialAsync(idConversacion.Value);
                await _chatService.MarcarComoLeidosAsync(idConversacion.Value, usuario.Id);

                var (idUCliente, idUVet) =
                    await _chatService.ObtenerUsuariosConversacionAsync(idConversacion.Value);
                int idOtro = usuario.Id == idUCliente ? idUVet : idUCliente;
                nombreDestinatario = await _chatService.ObtenerNombreUsuarioAsync(idOtro);
                imagenDestinatario = await _chatService.ObtenerImagenUsuarioAsync(idOtro);
            }
            else if (listaConv.Count > 0)
            {
                return RedirectToAction("Index", new { idConversacion = listaConv[0].IdConversacion });
            }

            var model = new ChatPageViewModel
            {
                IdUsuarioActual = usuario.Id,
                NombreUsuarioActual = usuario.Nombre,
                Conversaciones = listaConv,
                IdConversacionActiva = idConversacion,
                HistorialActivo = historial,
                NombreDestinatario = nombreDestinatario,
                ImagenDestinatario = imagenDestinatario
            };

            return View(model);
        }

        public async Task<IActionResult> NuevoChat()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth");

            var veterinarios = await _chatService.ObtenerVeterinariosDisponiblesAsync(usuario.Id);
            return View(veterinarios);
        }

        [HttpPost]
        public async Task<IActionResult> CrearChat(int idVeterinario)
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth");

            int? idCliente = await _chatService.ObtenerIdClientePorUsuarioAsync(usuario.Id);
            if (!idCliente.HasValue) return RedirectToAction("Index");

            ChatConversacion conversacion =
                await _chatService.ObtenerOCrearConversacionAsync(idCliente.Value, idVeterinario);

            var (idUsuarioCliente, idUsuarioVeterinario) =
                await _chatService.ObtenerUsuariosConversacionAsync(conversacion.Id);

            string nombreVet = await _chatService.ObtenerNombreUsuarioAsync(idUsuarioVeterinario);
            string? imagenVet = await _chatService.ObtenerImagenUsuarioAsync(idUsuarioVeterinario);

            await _hubContext.Clients.Group($"user-{idUsuarioVeterinario}")
                .SendAsync("NuevaConversacion", new
                {
                    IdConversacion = conversacion.Id,
                    NombreOtroUsuario = usuario.Nombre,
                    ImagenOtroUsuario = usuario.Imagen
                });

            // Notificar al cliente (ve los datos del veterinario)
            await _hubContext.Clients.Group($"user-{idUsuarioCliente}")
                .SendAsync("NuevaConversacion", new
                {
                    IdConversacion = conversacion.Id,
                    NombreOtroUsuario = nombreVet,
                    ImagenOtroUsuario = imagenVet
                });

            return RedirectToAction("Index", new { idConversacion = conversacion.Id });
        }
    }
}
