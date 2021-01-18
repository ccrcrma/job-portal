using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;

namespace job_portal.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string GetHumanFriendlyCreatedDate
        {
            get
            {
                var numberOfDays = (DateTime.UtcNow.Day - CreatedOn.Day);
                if (numberOfDays > 30)
                {
                    return CreatedOn.ToString("MMMM dd, yyyy");
                }
                return CreatedOn.Humanize();
            }
        }
    }
}