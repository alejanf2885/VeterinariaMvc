namespace VeterinariaMvc.Models.Chats
{
    public class VeterinarioDisponibleViewModel
    {
        public int IdVeterinario { get; set; }
        public string Nombre { get; set; }
        public string? Imagen { get; set; }
        public string NombreClinica { get; set; }
        public string NumeroColegiado { get; set; }
    }
}
