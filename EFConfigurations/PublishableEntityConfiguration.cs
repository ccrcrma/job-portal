using job_portal.Models;
using job_portal.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    public class PublishableEntityConfiguration<TEntity> : AuditableEntityConfiguration<TEntity> where TEntity : PublishableEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(publishableEntity => publishableEntity.Status)
                .HasColumnType("tinyint")
                .IsRequired()
                .HasDefaultValue(PublishedStatus.Draft);
            builder.HasQueryFilter(p => p.Status == PublishedStatus.Live);
            base.Configure(builder);

        }
    }
}