using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models.Chats;

namespace VeterinariaMvc.Repositories.Chats
{
    public class ChatRepository : IChatRepository
    {
        private readonly Context _context;

        public ChatRepository(Context context)
        {
            _context = context;
        }

        public async Task<ChatConversacion> ObtenerOCrearConversacionAsync(int idCliente, int idVeterinario)
        {
            // Buscamos si ya existe la conversación
            ChatConversacion conversacion = await _context.ChatConversaciones
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.IdVeterinario == idVeterinario);

            if (conversacion == null)
            {
                // Si no existe, la creamos
                conversacion = new ChatConversacion
                {
                    IdCliente = idCliente,
                    IdVeterinario = idVeterinario,
                    FechaCreacion = DateTime.Now
                };
                _context.ChatConversaciones.Add(conversacion);
                await _context.SaveChangesAsync();
            }

            return conversacion;
        }

        public async Task<ChatMensaje> GuardarMensajeAsync(int idConversacion, int idRemitente, string texto)
        {
            ChatMensaje mensaje = new ChatMensaje
            {
                IdConversacion = idConversacion,
                IdRemitente = idRemitente,
                Mensaje = texto,
                FechaEnvio = DateTime.Now,
                Leido = false 
            };

            _context.ChatMensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return mensaje;
        }

        public async Task<List<ChatMensaje>> ObtenerHistorialAsync(int idConversacion)
        {
            return await _context.ChatMensajes
                .Where(m => m.IdConversacion == idConversacion)
                .OrderBy(m => m.FechaEnvio)
                .ToListAsync();
        }

        public async Task MarcarComoLeidosAsync(int idConversacion, int idUsuarioReceptor)
        {
            List<ChatMensaje> mensajesNoLeidos = await _context.ChatMensajes
                .Where(m => m.IdConversacion == idConversacion
                         && m.IdRemitente != idUsuarioReceptor
                         && !m.Leido)
                .ToListAsync();

            if (mensajesNoLeidos.Any())
            {
                foreach (var msg in mensajesNoLeidos)
                {
                    msg.Leido = true;
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ChatConversacion> ObtenerConversacionPorIdAsync(int idConversacion)
        {
            return await _context.ChatConversaciones
                .FirstOrDefaultAsync(c => c.Id == idConversacion);
        }

        public async Task<(int IdUsuarioCliente, int IdUsuarioVeterinario)>
    ObtenerUsuariosConversacionAsync(int idConversacion)
        {
            var resultado = await (
                from c in _context.ChatConversaciones
                join cl in _context.Clientes on c.IdCliente equals cl.Id
                join v in _context.Veterinarios on c.IdVeterinario equals v.Id
                where c.Id == idConversacion
                select new
                {
                    IdUsuarioCliente = cl.IdUsuario,
                    IdUsuarioVeterinario = v.IdUsuario
                }
            ).FirstOrDefaultAsync();

            if (resultado == null)
                throw new Exception("Conversación no encontrada");

            return (resultado.IdUsuarioCliente, resultado.IdUsuarioVeterinario);
        }

        public async Task<List<ChatConversacion>> ObtenerConversacionesPorUsuarioAsync(int idUsuario)
        {
            int? idCliente = await _context.Clientes
                .Where(c => c.IdUsuario == idUsuario)
                .Select(c => (int?)c.Id)
                .FirstOrDefaultAsync();

            int? idVeterinario = await _context.Veterinarios
                .Where(v => v.IdUsuario == idUsuario)
                .Select(v => (int?)v.Id)
                .FirstOrDefaultAsync();

            return await _context.ChatConversaciones
                .Where(c => (idCliente.HasValue && c.IdCliente == idCliente.Value)
                          || (idVeterinario.HasValue && c.IdVeterinario == idVeterinario.Value))
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        public async Task<string> ObtenerNombreUsuarioAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Where(u => u.Id == idUsuario)
                .Select(u => u.Nombre)
                .FirstOrDefaultAsync() ?? "Desconocido";
        }

        public async Task<string?> ObtenerImagenUsuarioAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Where(u => u.Id == idUsuario)
                .Select(u => u.Imagen)
                .FirstOrDefaultAsync();
        }

        public async Task<int> ContarMensajesNoLeidosAsync(int idConversacion, int idUsuario)
        {
            return await _context.ChatMensajes
                .CountAsync(m => m.IdConversacion == idConversacion
                              && m.IdRemitente != idUsuario
                              && !m.Leido);
        }

        public async Task<ChatMensaje?> ObtenerUltimoMensajeAsync(int idConversacion)
        {
            return await _context.ChatMensajes
                .Where(m => m.IdConversacion == idConversacion)
                .OrderByDescending(m => m.FechaEnvio)
                .FirstOrDefaultAsync();
        }

        public async Task<List<VeterinarioDisponibleViewModel>> ObtenerVeterinariosDisponiblesAsync(int idUsuario)
        {
            int? idCliente = await ObtenerIdClientePorUsuarioAsync(idUsuario);

            var query = from v in _context.Veterinarios
                        join u in _context.Usuarios on v.IdUsuario equals u.Id
                        join c in _context.Clinicas on v.IdClinica equals c.Id
                        where u.Activo
                        select new VeterinarioDisponibleViewModel
                        {
                            IdVeterinario = v.Id,
                            Nombre = u.Nombre,
                            Imagen = u.Imagen,
                            NombreClinica = c.Nombre,
                            NumeroColegiado = v.NumeroColegiado
                        };

            var veterinarios = await query.ToListAsync();

            if (idCliente.HasValue)
            {
                var idsVetConChat = await _context.ChatConversaciones
                    .Where(conv => conv.IdCliente == idCliente.Value)
                    .Select(conv => conv.IdVeterinario)
                    .ToListAsync();

                veterinarios = veterinarios
                    .Where(v => !idsVetConChat.Contains(v.IdVeterinario))
                    .ToList();
            }

            return veterinarios;
        }

        public async Task<int?> ObtenerIdClientePorUsuarioAsync(int idUsuario)
        {
            return await _context.Clientes
                .Where(c => c.IdUsuario == idUsuario)
                .Select(c => (int?)c.Id)
                .FirstOrDefaultAsync();
        }
    }
}