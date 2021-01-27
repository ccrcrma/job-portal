using System;
using job_portal.Areas.Identity.Types;
using Microsoft.AspNetCore.Identity;

namespace job_portal.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public string MiddleName { get; set; }

        public string Name
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }





    }
}