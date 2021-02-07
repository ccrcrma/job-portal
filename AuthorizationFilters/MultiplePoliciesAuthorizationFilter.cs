using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace job_portal.AuthorizationFilters
{
    public class MultiplePoliciesAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string[] Policies { get; private set; }
        public bool AndPolicies { get; private set; }
        public MultiplePoliciesAuthorizationFilter(bool andPolicies, IAuthorizationService authorization, params string[] policies)
        {
            Policies = policies;
            AndPolicies = andPolicies;
            _authorization = authorization;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (AndPolicies)
            {
                foreach (var policy in Policies)
                {
                    var authorizationResult = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (!authorizationResult.Succeeded)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            else
            {
                foreach (var policy in Policies)
                {
                    var authorizationResult = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (authorizationResult.Succeeded)
                    {
                        return;
                    }
                }
                context.Result = new ForbidResult();
                return;
            }
        }

    }
}