using System;
using System.Collections.Generic;
using job_portal.Areas.Administration.Models;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Models;

namespace job_portal.ViewModels
{
    public class HomeViewModel
    {
        public List<JobCategory> Categories { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public SearchFilterViewModel Filter { get; set; } = new SearchFilterViewModel();
        
        
    }
}