using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using VeterinariaMvc.Hubs;
using VeterinariaMvc.Models.Chats;
using VeterinariaMvc.Services.Chats;

namespace VeterinariaMvc.Controllers
{
    [Authorize] 
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IAuthorizationService _authService; 
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(
            IChatService chatService,
            IAuthorizationService authService,
            IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _authService = authService;
            _hubContext = hubContext;
        }

        private int ObtenerIdUsuarioActual()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<IActionResult> Index(int? idConversacion)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            List<ChatConversacion> conversaciones =
                await _chatService.ObtenerConversacionesPorUsuarioAsync(idUsuario);

            var listaConv = new List<ChatConversacionItemViewModel>();
            foreach (var conv in conversaciones)
            {
                var (idUsuarioCliente, idUsuarioVeterinario) = await _chatService.ObtenerUsuariosConversacionAsync(conv.Id);

                int idOtroUsuario = idUsuario == idUsuarioCliente ? idUsuarioVeterinario : idUsuarioCliente;

                string nombre = await _chatService.ObtenerNombreUsuarioAsync(idOtroUsuario);
                string? imagen = await _chatService.ObtenerImagenUsuarioAsync(idOtroUsuario);
                int noLeidos = await _chatService.ContarMensajesNoLeidosAsync(conv.Id, idUsuario);
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

            if (idConversacion.HasValue)
            {
                var autorizacion = await _authService.AuthorizeAsync(User, idConversacion.Value, "PoliticaPermisoChat");

                if (!autorizacion.Succeeded)
                {
                    TempData["Error"] = "No tienes permiso para acceder a esta conversación.";
                    return RedirectToAction(nameof(Index), new { idConversacion = (int?)null });
                }

                historial = await _chatService.ObtenerHistorialAsync(idConversacion.Value);
                await _chatService.MarcarComoLeidosAsync(idConversacion.Value, idUsuario);

                var (idUCliente, idUVet) = await _chatService.ObtenerUsuariosConversacionAsync(idConversacion.Value);
                int idOtro = idUsuario == idUCliente ? idUVet : idUCliente;
                nombreDestinatario = await _chatService.ObtenerNombreUsuarioAsync(idOtro);
                imagenDestinatario = await _chatService.ObtenerImagenUsuarioAsync(idOtro);
            }
            else if (listaConv.Count > 0)
            {
                return RedirectToAction("Index", new { idConversacion = listaConv[0].IdConversacion });
            }

            var model = new ChatPageViewModel
            {
                IdUsuarioActual = idUsuario,
                NombreUsuarioActual = User.Identity?.Name ?? "Usuario",
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
            int idUsuario = ObtenerIdUsuarioActual();
            var veterinarios = await _chatService.ObtenerVeterinariosDisponiblesAsync(idUsuario);
            return View(veterinarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> CrearChat(int idVeterinario)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            int? idCliente = await _chatService.ObtenerIdClientePorUsuarioAsync(idUsuario);
            if (!idCliente.HasValue) return RedirectToAction("Index");

            ChatConversacion conversacion = await _chatService.ObtenerOCrearConversacionAsync(idCliente.Value, idVeterinario);

            var (idUsuarioCliente, idUsuarioVeterinario) = await _chatService.ObtenerUsuariosConversacionAsync(conversacion.Id);
            string nombreVet = await _chatService.ObtenerNombreUsuarioAsync(idUsuarioVeterinario);
            string? imagenVet = await _chatService.ObtenerImagenUsuarioAsync(idUsuarioVeterinario);
            string imagenUsuario = User.FindFirstValue("Imagen") ?? ""; 

            await _hubContext.Clients.Group($"user-{idUsuarioVeterinario}")
                .SendAsync("NuevaConversacion", new { IdConversacion = conversacion.Id, NombreOtroUsuario = User.Identity?.Name, ImagenOtroUsuario = imagenUsuario });

            await _hubContext.Clients.Group($"user-{idUsuarioCliente}")
                .SendAsync("NuevaConversacion", new { IdConversacion = conversacion.Id, NombreOtroUsuario = nombreVet, ImagenOtroUsuario = imagenVet });

            return RedirectToAction("Index", new { idConversacion = conversacion.Id });
        }
    }
}