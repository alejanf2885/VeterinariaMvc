using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Auth
{

    [Table("VW_AUTH_USUARIO")]
    public class AuthUsuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int Id { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("ACTIVO")]
        public bool Activo { get; set; }

        [Column("TIPOCREDENCIAL")]
        public string TipoCredencial { get; set; }

        [Column("PASSWORDHASH")]
        public string PasswordHash { get; set; }

    }
}
