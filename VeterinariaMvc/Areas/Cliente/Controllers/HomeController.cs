using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Chats;
using VeterinariaMvc.Models.Chats;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMascotasService _mascotasService;
        private readonly IConsultaService _consultaService;
        private readonly IChatService _chatService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public HomeController(
            IMascotasService mascotasService,
            IConsultaService consultaService,
            IChatService chatService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _mascotasService = mascotasService;
            _consultaService = consultaService;
            _chatService = chatService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        private async Task<UsuarioSessionDto> ObtenerUsuarioActualAsync()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();
            if (usuario == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");
            return usuario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();
            int idUsuario = usuarioActual.Id;

            List<MascotaResumenDto> mascotas = await _mascotasService.GetMascotasByUserAsync(idUsuario);
            List<ConsultaResumen> consultas = await _consultaService.GetConsultasDashboardAsync(idUsuario);
            List<ChatConversacion> conversaciones = await _chatService.ObtenerConversacionesPorUsuarioAsync(idUsuario);

            List<ChatConversacionItemViewModel> listaConv = new List<ChatConversacionItemViewModel>();
            int totalNoLeidos = 0;

            foreach (ChatConversacion conv in conversaciones)
            {
                (int idUsuarioCliente, int idUsuarioVeterinario) participantes = await _chatService.ObtenerUsuariosConversacionAsync(conv.Id);

                int idOtroUsuario = idUsuario == participantes.idUsuarioCliente
                    ? participantes.idUsuarioVeterinario
                    : participantes.idUsuarioCliente;

                string nombre = await _chatService.ObtenerNombreUsuarioAsync(idOtroUsuario);
                string? imagen = await _chatService.ObtenerImagenUsuarioAsync(idOtroUsuario);
                int noLeidos = await _chatService.ContarMensajesNoLeidosAsync(conv.Id, idUsuario);
                ChatMensaje? ultimo = await _chatService.ObtenerUltimoMensajeAsync(conv.Id);

                totalNoLeidos += noLeidos;

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

            DashboardViewModel model = new DashboardViewModel
            {
                usuario = usuarioActual,
                Mascotas = mascotas,
                Consultas = consultas,
                Conversaciones = listaConv,
                TotalMensajesNoLeidos = totalNoLeidos
            };

            return View(model);
        }
    }
}