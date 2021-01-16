using System;
using System.Collections.Generic;
using job_portal.Models;

namespace job_portal.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<Post> Posts { get; set; }
    }
}