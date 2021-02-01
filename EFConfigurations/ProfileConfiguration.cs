using job_portal.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {

            builder.Property(p => p.ImageName).HasMaxLength(100);
            builder.HasOne<ApplicationUser>().WithOne(u => u.Profile).HasForeignKey("Profile", "UserId").IsRequired();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}