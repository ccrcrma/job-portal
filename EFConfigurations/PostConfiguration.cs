using job_portal.Areas.Administration.Models;
using job_portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    public class PostConfiguration : PublishableEntityConfiguration<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);
            builder.ToTable("post");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Body).IsRequired();
            builder.HasMany<Tag>(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(j => j.ToTable("post_tags"));
            builder.Property(p => p.ImageName).IsRequired().HasMaxLength(100);
        }
    }
}