using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Chats
{
    [Table("CHAT_CONVERSACION")]
    public class ChatConversacion
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [Column("ID_VETERINARIO")]
        public int IdVeterinario { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }
    }
}