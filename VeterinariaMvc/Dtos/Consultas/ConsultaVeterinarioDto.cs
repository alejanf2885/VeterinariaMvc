using System;

namespace VeterinariaMvc.Dtos.Consultas
{
    public class ConsultaVeterinarioDto
    {
        public int IdConsulta { get; set; }
        public int IdClinica { get; set; }
        public string NombreClinica { get; set; }
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string ImagenMascota { get; set; }
        public int IdDueno { get; set; }
        public string NombreDueno { get; set; }
        public string TelefonoDueno { get; set; }
        public string EmailDueno { get; set; }
        public int IdVeterinario { get; set; }
        public DateTime FechaHoraConsulta { get; set; }
        public string Turno { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
    }
}
