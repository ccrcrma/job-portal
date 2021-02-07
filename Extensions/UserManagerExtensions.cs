using System.Security.Claims;
using System.Threading.Tasks;
using job_portal.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace job_portal.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByUsersAsync(this UserManager<ApplicationUser> input,
            ClaimsPrincipal user)
        {
            return await input.Users
                .Include(x => x.AppliedJobs)
                .Include(x => x.SavedJobs)
                .SingleOrDefaultAsync(x => x.NormalizedUserName == user.Identity.Name.ToUpper());
        }
    }
}