using System.Collections.Generic;
using job_portal.Util;
using static job_portal.Models.Job;

namespace job_portal.ViewModels
{
    public class SearchFilterViewModel
    {
        public string Category { get; set; }

        public List<Checkbox<JobType>> Type { get; set; } = new List<Checkbox<JobType>>
        {
            new Checkbox<JobType>
            {
                Text = JobType.Remote.ToString(),
                Value = JobType.Remote,
            },
            new Checkbox<JobType>
            {
                Text = JobType.Freelance.ToString(),
                Value = JobType.Freelance,
            },
            new Checkbox<JobType>
            {
                Text = JobType.PartTime.ToString(),
                Value = JobType.PartTime,
            },
            new Checkbox<JobType>
            {
                Text = JobType.FullTime.ToString(),
                Value = JobType.FullTime,
            }
        };

    }
}