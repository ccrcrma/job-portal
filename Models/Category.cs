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

    }

    public class PostCategory : Category { }
}