using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Chats
{
    [Table("CHAT_MENSAJE")]
    public class ChatMensaje
    {

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_CONVERSACION")]
        public int IdConversacion { get; set; }

        [Column("ID_REMITENTE")]
        public int IdRemitente { get; set; }

        [Column("MENSAJE")]
        public string Mensaje { get; set; }

        [Column("FECHA_ENVIO")]
        public DateTime FechaEnvio { get; set; }

        [Column("LEIDO")]
        public bool Leido { get; set; }
    }
}
