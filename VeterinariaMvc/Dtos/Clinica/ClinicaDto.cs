namespace VeterinariaMvc.Dtos.Clinica
{
    public class ClinicaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public string? Imagen { get; set; }
    }
}
