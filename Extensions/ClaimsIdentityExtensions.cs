using System.Linq;
using System.Security.Claims;

namespace job_portal.Extensions
{
    public static class ClaimsIdentityExtension
    {
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimsType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == claimsType);
            return claim?.Value;
        }
    }
}