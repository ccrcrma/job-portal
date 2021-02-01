using System.Collections.Generic;

namespace job_portal.Models
{
    public abstract class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

    }
    public class JobCategory : Category
    {
        public virtual List<Job> Jobs { get; set; }

    }

    public class PostCategory : Category { }
}