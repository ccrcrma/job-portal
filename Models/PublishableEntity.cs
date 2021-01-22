namespace job_portal.Models
{
    public enum PublishedStatus
    {
        Live = 1,
        Draft = 2
    }
    public abstract class PublishableEntity : AuditableEntity
    {
        public PublishedStatus Status { get; set; }

    }
}