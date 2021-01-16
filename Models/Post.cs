using System;

namespace job_portal.Models
{
    public class Post : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
}