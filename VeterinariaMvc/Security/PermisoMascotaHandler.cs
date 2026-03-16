using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Clinica;

namespace VeterinariaMvc.Security
{
    public class PermisoMascotaHandler : AuthorizationHandler<PermisoMascotaRequirement, MascotaDetalleDto>
    {
        private readonly IClinicaService _clinicaService;

        public PermisoMascotaHandler(IClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoMascotaRequirement requirement,
            MascotaDetalleDto mascota)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) return; 

            int idUsuario = int.Parse(userIdClaim);
            int idRol = string.IsNullOrEmpty(roleClaim) ? 0 : int.Parse(roleClaim);

            if (mascota.IdUsuario == idUsuario)
            {
                context.Succeed(requirement);
                return;
            }

            if (idRol == (int)Roles.Veterinario)
            {
                int? idClinicaVeterinario = await _clinicaService.ObtenerIdClinicaDeUsuarioAsync(idUsuario);
                if (idClinicaVeterinario.HasValue && idClinicaVeterinario.Value == mascota.IdClinica)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}