using job_portals.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace job_portal.Extensions
{
    public static class MiddlewaresExtension
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}