using System;
using System.Threading;
using System.Threading.Tasks;
using job_portal.Areas.Administration.Models;
using job_portal.Areas.Employer.Models;
using job_portal.Areas.Identity.Models;
using job_portal.Areas.Seeker.Models;
using job_portal.EFConfigurations;
using job_portal.Extensions;
using job_portal.Extensions.SoftDeleteQueryExtension;
using job_portal.Interfaces;
using job_portal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace job_portal.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                // .UseLazyLoadingProxies()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(category =>
            {
                category.ToTable("category")
                    .HasKey(c => c.Id);
                category.HasDiscriminator(c => c.Type)
                    .HasValue<JobCategory>("job")
                    .HasValue<PostCategory>("post");
                category.Property(c => c.Type).IsRequired().HasMaxLength(100);
                category.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Testimonial>(testimonial =>
            {
                testimonial.ToTable("testimonial").HasKey(t => t.Id);
                testimonial.Property(t => t.Name).IsRequired().HasMaxLength(100);
                testimonial.Property(t => t.Designation).IsRequired().HasMaxLength(100);
                testimonial.Property(t => t.Description).IsRequired();
                testimonial.Property(t => t.ImageName).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Tag>(tag =>
            {
                tag.ToTable("tag").HasKey(t => t.Id);
                tag.Property(tag => tag.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConifguration());

            modelBuilder.ApplyConfiguration(new ProfileConfiguration());

            modelBuilder.Entity<ApplicationUser>(applicationUser =>
            {
                applicationUser.Property(u => u.FirstName).HasMaxLength(100);
                applicationUser.Property(u => u.LastName).HasMaxLength(100);
                applicationUser.Property(u => u.MiddleName).HasMaxLength(100);
                applicationUser.Property(u => u.DOB).IsRequired(false).HasColumnType("date");
                applicationUser.Property(u => u.Gender).IsRequired(false).HasColumnType("tinyint(2)").HasMaxLength(100);
                applicationUser.Property(u => u.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");

            });

            modelBuilder.Entity<SavedJob>(sj =>
            {
                sj.Property(sj => sj.AddedAt).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
                sj.HasOne<ApplicationUser>(sj => sj.User)
                    .WithMany(u => u.SavedJobs)
                    .HasForeignKey(sj => sj.UserId)
                    .IsRequired();
                sj.HasOne<Job>(sj => sj.Job)
                    .WithMany()
                    .HasForeignKey(sj => sj.JobId)
                    .IsRequired();
                sj.HasKey(sj => new { sj.UserId, sj.JobId });
            });

            modelBuilder.ApplyConfiguration(new AppliedJobConfiguration());
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsSubclassOf(typeof(PublishableEntity)) && typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteAndPublishedQueryFilter();
                }
                else if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is AuditableEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.UpdatedOn = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("CreatedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.CreatedOn = utcNow;
                            trackable.UpdatedOn = utcNow;
                            break;
                    }
                }
            }
        }


        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess);
        }


    }
}