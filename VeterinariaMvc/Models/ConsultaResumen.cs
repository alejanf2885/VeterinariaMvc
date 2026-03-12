using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Dtos.Consultas
{
    namespace VeterinariaMvc.Dtos.Consulta
    {
        [Table("VW_CONSULTAS_DETALLE")]
        public class ConsultaResumen
        {
            [Key] // Identificador lógico para EF Core
            [Column("IdConsulta")]
            public int IdConsulta { get; set; }

            [Column("IdUsuario")]
            public int IdUsuario { get; set; }

            [Column("NombreMascota")]
            public string NombreMascota { get; set; }

            [Column("NombreClinica")]
            public string NombreClinica { get; set; }

            [Column("Fecha")]
            public DateTime Fecha { get; set; }

            [Column("Motivo")]
            public string Motivo { get; set; }

            [Column("Estado")]
            public string Estado { get; set; }
        }
    }
}

