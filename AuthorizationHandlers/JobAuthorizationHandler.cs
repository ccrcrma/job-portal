using System.Threading.Tasks;
using job_portal.Requirements;
using static job_portal.Constants.Constant;
using Microsoft.AspNetCore.Authorization;

namespace job_portal.AuthorizationHandlers
{
    public class JobAuthorizationHandler : AuthorizationHandler<OwnsJobRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnsJobRequirement requirement)
        {
            if (context.User.IsInRole(AdminRole))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            if (context.User.HasClaim(c => c.Type == CompanyNameClaim))
            {
                var companyName = context.User.FindFirst(c => c.Type == CompanyNameClaim).Value;
                if (requirement.CompanyName == companyName)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}