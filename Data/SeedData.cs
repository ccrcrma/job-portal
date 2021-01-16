using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using job_portal.Models;

namespace job_portal.Data
{
    public class SeedData
    {
        private readonly ApplicationContext _context;

        public SeedData(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<ApplicationContext>();

        }

        public async Task SeedAll()
        {
            await SeedJobCategoriesAsync();
            await SeedPostCategories();
            await seedTestimonialsAsync();
            await SeedPostsAsync();
        }
        public async Task SeedJobCategoriesAsync()
        {
            if (await _context.Set<JobCategory>().CountAsync() == 0)
            {
                Category[] categories = {
                    new JobCategory{Name="Design & Creative"},
                    new JobCategory{Name="Design & Development"},
                    new JobCategory{Name="Sales & Marketing"},
                    new JobCategory{Name="Mobile Application"},
                    new JobCategory{Name="Construction"},
                    new JobCategory{Name="Information Technology"},
                    new JobCategory{Name="Real Estate"},
                    new JobCategory{Name="Content Writer"}
                    };


                await _context.Categories.AddRangeAsync(
                    categories
                );
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedPostCategories()
        {
            if (await _context.Set<PostCategory>().CountAsync() == 0)
            {
                PostCategory[] categories = {
                    new PostCategory{Name="Restaurant Food"},
                    new PostCategory{Name="Travel News"},
                    new PostCategory{Name="Modern Technology"},
                    new PostCategory{Name="Product"},
                    new PostCategory{Name="Inspiration"},
                    new PostCategory{Name="Health Care"},
                };
                await _context.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task seedTestimonialsAsync()
        {
            if (await _context.Testimonials.CountAsync() == 0)
            {
                Testimonial[] testimonials = {
                    new Testimonial{
                        Name = "Margaret Lawson",
                        Designation = "Creative Director",
                        Description= " I am at an age  where I just want to be fit and " +
                        "our bodies are our responsibility! So start caring for your body " +
                        "and it will care for you.Eat clean it will work for you and workout hard."
                    },
                    new Testimonial{
                        Name = "Shishir Sharma",
                        Designation = "Billionaire",
                        Description= " I am at an age  where I just want to be fit and " +
                        "our bodies are our responsibility! So start caring for your body " +
                        "and it will care for you.Eat clean it will work for you and workout hard."
                    },
                    };
                await _context.Testimonials.AddRangeAsync(testimonials);
            };
            await _context.SaveChangesAsync();
        }

        public async Task SeedPostsAsync()
        {
            if (await _context.Posts.CountAsync() == 0)
            {
                Post[] posts = {
                    new Post{
                        Title = "FootPrints in Time is Perfect House in Kurashiki",
                        Body = "some random body for the text which is meaningless"
                    },
                    new Post{
                        Title = "FootPrints in Time is Perfect House in Kurashiki",
                        Body = "some random body for the text which is meaningless"
                    },

                };
                await _context.Posts.AddRangeAsync(posts);
                await _context.SaveChangesAsync();
            }
        }
    }
}