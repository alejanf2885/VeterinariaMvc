namespace VeterinariaMvc.Dtos.Mascota
{
    public class MascotaDetalleDto
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public string? ImagenMascota { get; set; }
        public decimal? Peso { get; set; }

        public string? NombreClinica { get; set; }
        public string? DireccionClinica { get; set; }
        public string? EstadoEnClinica { get; set; }
    }
}
