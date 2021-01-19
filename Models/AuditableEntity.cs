using System;
namespace job_portal.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}