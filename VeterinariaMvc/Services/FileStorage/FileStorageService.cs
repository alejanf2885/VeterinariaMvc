using VeterinariaMvc.Enums;

namespace VeterinariaMvc.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {

        private IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Task BorrarArchivoAsync(string rutaRelativa)
        {
            if (string.IsNullOrEmpty(rutaRelativa))
            {
                return Task.CompletedTask;
            }

            // 1. Evitar borrar la imagen por defecto
            // Si la ruta contiene "default-avatar", salimos para no dejar al sistema sin la imagen base
            if (rutaRelativa.Contains("default-avatar.png", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            try
            {
                string rutaLimpia = rutaRelativa.Replace("/", Path.DirectorySeparatorChar.ToString()).TrimStart(Path.DirectorySeparatorChar);
                string rutaFisica = Path.Combine(_env.WebRootPath, rutaLimpia);

                if (File.Exists(rutaFisica))
                {
                    File.Delete(rutaFisica);
                }
            }
            catch (Exception)
            {
                // No queremos que esto interrumpa el flujo de la aplicación, así que simplemente ignoramos el error.
            }

            return Task.CompletedTask;
        }

        public async Task<string> GuardarArchivoAsync(Stream contenido, string nombreArchivo, CarpetaDestino carpeta)
        {

            string nombreCarpeta = carpeta.ToString().ToLower();

            string rutaCarpeta = Path.Combine(_env.WebRootPath, "images", nombreCarpeta);
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            string rutaFisica = Path.Combine(rutaCarpeta, nombreArchivo);

            using (var fileStream = new FileStream(rutaFisica, FileMode.Create))
            {
                await contenido.CopyToAsync(fileStream);
            }

            return $"/images/{nombreCarpeta}/{nombreArchivo}";
        }
    }
}
