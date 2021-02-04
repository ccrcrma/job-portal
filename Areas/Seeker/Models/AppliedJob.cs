using System;
using job_portal.Areas.Identity.Models;
using job_portal.Models;

namespace job_portal.Areas.Seeker.Models
{
    public class AppliedJob
    {
        public int JobId { get; set; }
        public string UserId { get; set; }
        public DateTime AppliedAt { get; set; }
        public virtual Job Job { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}