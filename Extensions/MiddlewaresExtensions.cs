using job_portal.Middlewares;
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