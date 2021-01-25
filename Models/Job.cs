using System;
using System.ComponentModel.DataAnnotations;
using job_portal.Interfaces;
using job_portal.ViewModels;

namespace job_portal.Models
{
    public class Job : PublishableEntity, ISoftDelete
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
        public bool IsSoftDeleted { get ; set; }

        public JobViewModel ToViewModel()
        {
            return new JobViewModel
            {
                Title = Title,
                Description = Description,
                Id = Id,
                Category = Category.Id,
                Type = Type,
                Vacancy = Vacancy,
                Deadline = Deadline,
                Location = Location,
                ExperienceRequired = ExperienceRequired
            };
        }
    }
}