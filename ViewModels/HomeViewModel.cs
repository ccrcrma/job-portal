using System;
using System.Collections.Generic;
using job_portal.Models;

namespace job_portal.ViewModels
{
    public class HomeViewModel
    {
        public List<JobCategory> Categories { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<Post> Posts { get; set; }
        public SearchFilterViewModel Filter { get; set; } = new SearchFilterViewModel();
        
        
    }
}