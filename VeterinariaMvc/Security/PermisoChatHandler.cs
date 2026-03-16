using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using VeterinariaMvc.Services.Chats;

namespace VeterinariaMvc.Security
{
    public class PermisoChatHandler : AuthorizationHandler<PermisoChatRequirement, int>
    {
        private readonly IChatService _chatService;

        public PermisoChatHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoChatRequirement requirement,
            int idConversacion)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return;

            int idUsuarioActual = int.Parse(userIdClaim);

            bool esParticipante = await _chatService.EsParticipanteDeConversacionAsync(idConversacion, idUsuarioActual);

            if (esParticipante)
            {
                context.Succeed(requirement);
            }
        }
    }
}