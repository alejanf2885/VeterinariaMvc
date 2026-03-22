using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VeterinariaMvc.Dtos.Mascota;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class EditarMascotaViewModel
    {
        [ValidateNever] 
        public CatalogosMascotaViewModels Catalogos { get; set; }

        public MascotaEditDto Formulario { get; set; }
    }
}

