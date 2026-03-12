using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        private  IEstadoUsuarioService _estadoUsuario;
        private  IMascotasService _mascotasService;
        private  IConsultaService _consultaService;
        private  IChatService _chatService;

        public HomeController(
            IEstadoUsuarioService estadoUsuario,
            IMascotasService mascotasService,
            IConsultaService consultaService,
            IChatService chatService)
        {
            this._estadoUsuario = estadoUsuario;
            _mascotasService = mascotasService;
            _consultaService = consultaService;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            List<MascotaResumenDto> mascotas = await _mascotasService.GetMascotasByUserAsync(usuario.Id);
            List<ConsultaResumen> consultas = await _consultaService.GetConsultasDashboardAsync(usuario.Id);

            List<ChatConversacion> conversaciones =
                await _chatService.ObtenerConversacionesPorUsuarioAsync(usuario.Id);

            var listaConv = new List<ChatConversacionItemViewModel>();
            int totalNoLeidos = 0;
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

            DashboardViewModel model = new DashboardViewModel();

            model.usuario = usuario;
            model.Mascotas = mascotas;
            model.Consultas = consultas;
            model.Conversaciones = listaConv;
            model.TotalMensajesNoLeidos = totalNoLeidos;

            return View(model);
        }
    }
}