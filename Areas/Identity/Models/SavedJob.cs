using System;
using job_portal.Models;

namespace job_portal.Areas.Identity.Models
{
    public class SavedJob
    {
        public virtual ApplicationUser User { get; set; }
        public virtual Job Job { get; set; }
        public string UserId { get; set; }
        public int JobId { get; set; }
        public DateTime AddedAt { get; set; }

    }
}