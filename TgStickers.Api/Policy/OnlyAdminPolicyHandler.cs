using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TgStickers.Api.Services;

namespace TgStickers.Api.Policy
{
    public class OnlyAdminPolicyHandler : AuthorizationHandler<OnlyAdminPolicyRequirement>
    {
        private readonly CurrentAdminProvider _currentAdminProvider;

        public OnlyAdminPolicyHandler(CurrentAdminProvider currentAdminProvider)
        {
            _currentAdminProvider = currentAdminProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyAdminPolicyRequirement requirement)
        {
            await _currentAdminProvider.ProviderCurrentAdminAsync();
        }
    }
}