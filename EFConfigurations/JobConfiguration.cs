using job_portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    public class JobConfiguration : AuditableEntityConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            base.Configure(builder);
            builder.ToTable("job").HasKey(j => j.Id);
            builder.Property(j => j.Title).IsRequired().HasMaxLength(200);
            builder.Property(j => j.Location).HasMaxLength(100).IsRequired();
            builder.Property(j => j.Salary).IsRequired().HasDefaultValue("Negotiable").HasMaxLength(20);
            builder.Property(j => j.Type).IsRequired().HasColumnType("tinyint");

            builder.HasOne<JobCategory>(j => j.Category)
                .WithMany(jc => jc.Jobs)
                .IsRequired()
                .HasForeignKey("category_id");
            builder.Property<int>("category_id").HasDefaultValue(17);

        }
    }
}