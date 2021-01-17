using System.Collections.Generic;

namespace job_portal.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}