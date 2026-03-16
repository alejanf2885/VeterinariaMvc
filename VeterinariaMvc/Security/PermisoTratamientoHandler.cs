using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Tratamiento;

namespace VeterinariaMvc.Security
{
    public class PermisoTratamientoHandler : AuthorizationHandler<PermisoTratamientoRequirement, TratamientoDto>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoTratamientoRequirement requirement,
            TratamientoDto tratamiento)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Task.CompletedTask;
            }

            int idUsuario = int.Parse(userIdClaim);

            int idRol = string.IsNullOrEmpty(roleClaim) ? 0 : int.Parse(roleClaim);

            if (tratamiento.IdUsuario == idUsuario)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (idRol == 2 && tratamiento.IdVeterinario == idUsuario)
            {
                context.Succeed(requirement); 
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}