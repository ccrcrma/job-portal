using System;
using job_portal.Areas.Identity.Types;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using job_portal.Areas.Seeker.ViewModels;
using job_portal.Areas.Seeker.Models;

namespace job_portal.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public Gender? Gender { get; set; }
        public string MiddleName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }

        public virtual List<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
        public virtual List<AppliedJob> AppliedJobs { get; set; } = new List<AppliedJob>();
        public virtual Profile Profile { get; set; }

        public void SaveJob(int jobId)
        {
            SavedJobs.Add(new SavedJob { JobId = jobId, UserId = Id });
        }

        public bool IsSaved(int jobId)
        {
            return SavedJobs.Any(j => j.JobId == jobId);
        }

        public bool IsApplied(int jobId)
        {
            return AppliedJobs.Any(j => j.JobId == jobId);
        }

        internal void RemoveJob(int jobId)
        {
            var job = SavedJobs.FirstOrDefault(j => j.JobId == jobId);
            if (job != null)
            {
                SavedJobs.Remove(job);
            }
        }

        public BasicProfileInfoViewModel GetProfileInfo()
        {
            return new BasicProfileInfoViewModel
            {
                Experience = Profile.Experience,
                Bio = Profile.Bio,
                Address = Profile.Address,
                PhoneNumber = PhoneNumber
            };
        }

    }
}