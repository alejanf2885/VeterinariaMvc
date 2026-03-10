namespace VeterinariaMvc.Dtos.Tratamiento
{
    public class TratamientoDto
    {
        public int Id { get; set; }
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public int IdVeterinario { get; set; }
        public string NombreVeterinario { get; set; }
        public int? IdConsulta { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public List<SeguimientoDto> Seguimientos { get; set; } = new();
    }
}
