using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("VW_MASCOTAS_DETALLE")]
    public class MascotaDetalle
    {
        [Key]
        [Column("IDMASCOTA")]
        public int IdMascota { get; set; }

        [Column("NOMBREMASCOTA")]
        public string NombreMascota { get; set; }

        [Column("ESPECIE")]
        public string Especie { get; set; }

        [Column("RAZA")]
        public string Raza { get; set; }

        [Column("IMAGENMASCOTA")]
        public string? ImagenMascota { get; set; }

        [Column("PESO")]
        public double? Peso { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("NOMBRECLINICA")]
        public string? NombreClinica { get; set; }

        [Column("DIRECCIONCLINICA")]
        public string? DireccionClinica { get; set; }

        [Column("ESTADOENCLINICA")]
        public string? EstadoEnClinica { get; set; }
    }
}