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
        public string Salary { get; set; }

        public string GetFormattedSalary
        {
            get
            {
                if (Salary == "Negotiable") return Salary;
                string FormattedSalary = "$" + Salary;
                if (FormattedSalary.Contains("-"))
                {
                    FormattedSalary = FormattedSalary.Insert(FormattedSalary.IndexOf("-") + 1, "$");
                }
                return FormattedSalary;
            }
        }
        public JobType Type { get; set; }
        public JobCategory Category { get; set; }

        public int ExperienceRequired { get; set; }

    }
}