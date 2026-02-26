namespace VeterinariaMvc.Services.Archivos
{
    public class NombreArchivoService : INombreArchivoService
    {
        public string GenerarNombreUnico(string nombreOriginal, string sufijo = "", string extensionDeseada = null)
        {
            if (string.IsNullOrWhiteSpace(nombreOriginal))
            {
                throw new ArgumentNullException(nameof(nombreOriginal),
                    "El nombre original es obligatorio.");
            }

            // Determinar extensión
            string extension = string.IsNullOrEmpty(extensionDeseada)
                ? Path.GetExtension(nombreOriginal)
                : extensionDeseada;

            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidOperationException($"No se pudo determinar una extensión válida para el archivo: {nombreOriginal}");
            }


            // Si la extension no contiene punto se lo añadimos
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string identificador = Guid.NewGuid().ToString("N");
            string sufijoFormateado = string.IsNullOrEmpty(sufijo) ? "" : $"-{sufijo}";

            string resultado = $"{identificador}{sufijoFormateado}{extension}";
            return resultado;
        }
    }
}
