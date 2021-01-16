using System;
using System.Threading;
using System.Threading.Tasks;
using job_portal.EFConfigurations;
using job_portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace job_portal.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
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
            });
            modelBuilder.ApplyConfiguration(new PostConfiguration());
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