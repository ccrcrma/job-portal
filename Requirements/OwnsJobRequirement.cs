using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace job_portal.Requirements
{
    public class OwnsJobRequirement : IAuthorizationRequirement
    {
        public string CompanyName { get; set; }

        public OwnsJobRequirement(string companyName)
        {
            CompanyName = companyName;
        }
    }
}