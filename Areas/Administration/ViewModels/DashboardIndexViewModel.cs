using System.Collections.Generic;
using job_portal.Areas.Administration.Models;
using job_portal.Models;

namespace job_portal.Areas.Administration.ViewModels
{
    public class DashboardIndexViewModel
    {
        public List<Job> Jobs { get; set; }
        public List<Post> Posts { get; set; }
        
    }
}