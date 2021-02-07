using job_portal.Areas.Employer.Models;

namespace job_portal.DTOs
{
    public class JobDTO
    {
        public string CreatedDate { get; set; }
        public string Position { get; set; }
        public Company Company { get; set; }
        public object Status { get; set; }
        public string Url { get; set; }

    }
}