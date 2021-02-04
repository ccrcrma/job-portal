using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using job_portal.Areas.Employer.ViewModels;
using job_portal.Models;

namespace job_portal.Areas.Employer.Models
{
    public class Company
    {
        public const string BaseBrandImageDirectory = "img/companies";
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Info { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Slogan { get; set; }
        public string BrandImage { get; set; }
        public string GetBrandImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(BrandImage)) return string.Empty;
                return Path.Combine(BaseBrandImageDirectory, BrandImage);
            }
        }
        public virtual List<Job> Jobs { get; set; }

        public CompanyViewModel ToVm()
        {
            return new CompanyViewModel
            {
                Id = Id,
                Name = Name,
                Location = Location,
                Phone = Phone,
                Website = Website,
                Slogan = Slogan,
                ImagePath = GetBrandImagePath,
                Info = Info
            };
        }

    }
}