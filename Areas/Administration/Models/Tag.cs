using System.Collections.Generic;

namespace job_portal.Areas.Administration.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}