using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using job_portal.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using static job_portal.Models.Job;

namespace job_portal.ViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Location { get; set; }

        [Range(minimum: 1000, maximum: 1000000)]
        public decimal? SalaryMin { get; set; }

        [SalaryGreaterThan("SalaryMin", ErrorMessage = "Max Salary must be greater than min Salary")]
        public decimal? SalaryMax { get; set; }
        public JobType Type { get; set; }
        public int Category { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Description { get; set; }
        [Range(minimum: 1, maximum: 50)]
        public int Vacancy { get; set; }

        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; } = DateTime.UtcNow;

        [Display(Name = "Experience In Years")]
        [Range(minimum: 1, maximum: 30)]
        public int ExperienceRequired { get; set; }

        public List<SelectListItem> JobCategories = new List<SelectListItem>() {
            new SelectListItem{Text = "random", Value ="Cat1"},
            new SelectListItem{Text = "random2", Value ="Cat1"},
        };

    }
}