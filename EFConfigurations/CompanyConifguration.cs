using job_portal.Areas.Employer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace job_portal.EFConfigurations
{
    internal class CompanyConifguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("company");
            builder.Property(c => c.Name).HasMaxLength(300).IsRequired();
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Location).HasMaxLength(300);
            builder.Property(c => c.Info);
            builder.Property(c => c.BrandImage).HasMaxLength(200);
            builder.Property(c => c.Slogan).HasMaxLength(200);
            builder.Property(c => c.Phone).HasMaxLength(50);
            builder.Property(c => c.Website).HasMaxLength(100);

        }
    }
}