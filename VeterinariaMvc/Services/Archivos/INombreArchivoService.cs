namespace VeterinariaMvc.Services.Archivos
{
    public interface INombreArchivoService
    {
        string GenerarNombreUnico(string nombreOriginal, string sufijo = "", string extensionDeseada = null);
    }
}
