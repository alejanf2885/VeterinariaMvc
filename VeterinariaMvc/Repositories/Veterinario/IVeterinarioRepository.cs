namespace VeterinariaMvc.Repositories.Clinica
{
    public interface IVeterinarioRepository
    {
        Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario);
    }
}
