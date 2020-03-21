using Microsoft.AspNetCore.Authorization;

namespace TgStickers.Api.Policy
{
    public class OnlyAdminAttribute : AuthorizeAttribute
    {
        public OnlyAdminAttribute(): base(OnlyAdminPolicyRequirement.PolicyName)
        {
        }
    }
}