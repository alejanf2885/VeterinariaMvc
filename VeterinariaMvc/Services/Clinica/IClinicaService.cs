namespace VeterinariaMvc.Services.Clinica
{
    public interface IClinicaService
    {
        Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario);
    }
}
