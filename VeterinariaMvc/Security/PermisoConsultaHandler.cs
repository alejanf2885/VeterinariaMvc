using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Security
{
    public class PermisoConsultaHandler : AuthorizationHandler<PermisoConsultaRequirement, ConsultaResumen>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoConsultaRequirement requirement,
            ConsultaResumen consulta)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Task.CompletedTask;
            }

            int idUsuario = int.Parse(userIdClaim);

            if (consulta.IdUsuario == idUsuario)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            return Task.CompletedTask; 
        }
    }
}