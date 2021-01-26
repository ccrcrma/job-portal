using job_portal.Types;

namespace job_portal.Models
{

    public abstract class PublishableEntity : AuditableEntity
    {
        public PublishedStatus Status { get; set; }

        public void ChangePublishedStatus()
        {
            Status = Status == PublishedStatus.Live ? PublishedStatus.Draft : PublishedStatus.Live;
        }

    }
}