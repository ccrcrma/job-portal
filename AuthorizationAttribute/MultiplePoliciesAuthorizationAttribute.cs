using job_portal.AuthorizationFilters;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.AuthorizationAttribute
{
    public class MultiplePoliciesAuthorizationAttribute : TypeFilterAttribute
    {
        public MultiplePoliciesAuthorizationAttribute(bool andPolicies = false, params string[] policies) : base(typeof(MultiplePoliciesAuthorizationFilter))
        {
            Arguments = new object[] { policies, andPolicies };
        }
    }
}