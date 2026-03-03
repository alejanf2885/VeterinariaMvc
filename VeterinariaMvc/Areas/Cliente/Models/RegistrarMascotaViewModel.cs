using VeterinariaMvc.Dtos.Mascota;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class RegistrarMascotaViewModel
    {
        public CatalogosMascotaViewModels Catalogos { get; set; }

        public MascotaRegisterDto Formulario { get; set; }
    }
}
