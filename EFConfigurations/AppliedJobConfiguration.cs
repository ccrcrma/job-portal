using job_portal.Areas.Identity.Models;
using job_portal.Areas.Seeker.Models;
using job_portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    internal class AppliedJobConfiguration : IEntityTypeConfiguration<AppliedJob>
    {

        public void Configure(EntityTypeBuilder<AppliedJob> builder)
        {
            builder.HasKey(aj => new { aj.JobId, aj.UserId });
            builder.HasOne<ApplicationUser>(aj => aj.User)
                .WithMany(u => u.AppliedJobs)
                .HasForeignKey(aj => aj.UserId)
                .IsRequired();
            builder.HasOne<Job>(aj => aj.Job)
                .WithMany(j => j.Appliers)
                .HasForeignKey(aj => aj.JobId)
                .IsRequired();
            builder.Property(aj => aj.AppliedAt)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}