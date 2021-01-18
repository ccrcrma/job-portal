using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using job_portal.Extensions;
using job_portal.Util;
using static job_portal.Models.Job;

namespace job_portal.ViewModels
{
    public class SearchFilterViewModel
    {
        public enum PostedDuration
        {
            Any = 100,
            Today = 0,

            [Display(Name = "Last 2 days")]
            LastTwoDays = 2,

            [Display(Name = "Last 3 days")]
            LastThreeDays = 3,

            [Display(Name = "Last 5 days")]
            LastFiveDays = 5,

            [Display(Name = "Last 10 days")]
            LastTenDays = 10,
        }

        public enum ExperienceLevel
        {
            [Display(Name = "1-2 Years")]
            A,
            [Display(Name = "2-3 Years")]
            B,
            [Display(Name = "3-6 Years")]
            C,
            [Display(Name = "6 +")]
            D
        }
        public string Category { get; set; }

        public List<Checkbox<JobType>> Type { get; set; } = new List<Checkbox<JobType>>
        {
            new Checkbox<JobType>
            {
                Text = JobType.Remote.GetDisplayName(),
                Value = JobType.Remote,
            },
            new Checkbox<JobType>
            {
                Text = JobType.Freelance.GetDisplayName(),
                Value = JobType.Freelance,
            },
            new Checkbox<JobType>
            {
                Text = JobType.PartTime.GetDisplayName(),
                Value = JobType.PartTime,
            },
            new Checkbox<JobType>
            {
                Text = JobType.FullTime.GetDisplayName(),
                Value = JobType.FullTime,
            }
        };

        public List<Checkbox<ExperienceLevel>> Experience { get; set; } = new List<Checkbox<ExperienceLevel>>
        {
            new Checkbox<ExperienceLevel>{
                Text= ExperienceLevel.A.GetDisplayName(),
                Value= ExperienceLevel.A,
            },
             new Checkbox<ExperienceLevel>{
                Text= ExperienceLevel.B.GetDisplayName(),
                Value= ExperienceLevel.B,
            },
             new Checkbox<ExperienceLevel>{
                Text= ExperienceLevel.C.GetDisplayName(),
                Value= ExperienceLevel.C,
            },
             new Checkbox<ExperienceLevel>{
                Text= ExperienceLevel.D.GetDisplayName(),
                Value= ExperienceLevel.D,
            },
        };


        public List<Checkbox<PostedDuration>> PostedWithin { get; set; } = new List<Checkbox<PostedDuration>>
        {
            new Checkbox<PostedDuration>{
                Text = PostedDuration.Any.GetDisplayName(),
                Value = PostedDuration.Any
            },
               new Checkbox<PostedDuration>{
                Text = PostedDuration.Today.GetDisplayName(),
                Value = PostedDuration.Today
            },
               new Checkbox<PostedDuration>{
                Text = PostedDuration.LastTwoDays.GetDisplayName(),
                Value = PostedDuration.LastTwoDays
            },
               new Checkbox<PostedDuration>{
                Text = PostedDuration.LastThreeDays.GetDisplayName(),
                Value = PostedDuration.LastThreeDays
            },
               new Checkbox<PostedDuration>{
                Text = PostedDuration.LastFiveDays.GetDisplayName(),
                Value = PostedDuration.LastFiveDays
            },
               new Checkbox<PostedDuration>{
                Text = PostedDuration.LastTenDays.GetDisplayName(),
                Value = PostedDuration.LastTenDays
            }
        };
    }
}