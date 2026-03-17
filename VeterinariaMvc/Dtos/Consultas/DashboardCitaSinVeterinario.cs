using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Dtos.Consultas
{
    namespace VeterinariaMvc.Dtos.Consulta
    {
        [Table("VW_DASHBOARD_CITAS_SIN_VETERINARIO")]
        public class DashboardCitaSinVeterinario
        {
            [Key]
            [Column("IdConsulta")]
            public int IdConsulta { get; set; }

            [Column("IdClinica")]
            public int IdClinica { get; set; }

            [Column("NombreClinica")]
            public string? NombreClinica { get; set; }

            [Column("IdMascota")]
            public int IdMascota { get; set; }

            [Column("NombreMascota")]
            public string? NombreMascota { get; set; }

            [Column("ImagenMascota")]
            public string? ImagenMascota { get; set; }

            [Column("IdDueno")]
            public int IdDueno { get; set; }

            [Column("NombreDueno")]
            public string? NombreDueno { get; set; }

            [Column("TelefonoDueno")]
            public string? TelefonoDueno { get; set; }

            [Column("EmailDueno")]
            public string? EmailDueno { get; set; }

            [Column("FechaHoraConsulta")]
            public DateTime FechaHoraConsulta { get; set; }

            [Column("Turno")]
            public string? Turno { get; set; }

            [Column("Motivo")]
            public string? Motivo { get; set; }

            [Column("Estado")]
            public string? Estado { get; set; }
        }
    }
}

