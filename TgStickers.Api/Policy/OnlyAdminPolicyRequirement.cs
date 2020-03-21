using Microsoft.AspNetCore.Authorization;

namespace TgStickers.Api.Policy
{
    public class OnlyAdminPolicyRequirement : IAuthorizationRequirement
    {
        public const string PolicyName = "OnlyAdminPolicy";
    }
}