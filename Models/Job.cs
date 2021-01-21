using System;
using System.ComponentModel.DataAnnotations;

namespace job_portal.Models
{
    public class Job : AuditableEntity
    {
        public int Id { get; set; }
        public enum JobType
        {

            [Display(Name = "Full Time")]
            FullTime = 1,
            [Display(Name = "Part Time")]
            PartTime,
            Remote,
            Freelance
        }
        public string Title { get; set; }
        public string Location { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string GetFormattedSalary
        {
            get
            {
                if (SalaryMin == null) return "Negotiable";
                string FormattedSalary = $"${Math.Round(SalaryMin.GetValueOrDefault())}";
                if (SalaryMax != null)
                    FormattedSalary = FormattedSalary + $" - ${Math.Round(SalaryMax.GetValueOrDefault())}";
                return FormattedSalary;
            }
        }
        public JobType Type { get; set; }
        public JobCategory Category { get; set; }
        public string Description { get; set; }
        public int Vacancy { get; set; }
        public DateTime Deadline { get; set; }
        public int ExperienceRequired { get; set; }

    }
}