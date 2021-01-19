using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using job_portal.Models;
using System.Collections.Generic;

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
            await SeedTags();
            await SeedPostsAsync();
            await SeedJobs();
        }

        private async Task SeedJobs()
        {
            if (await _context.Jobs.CountAsync() <= 0)
            {
                Job[] jobs =
                {
                    new Job
                    {
                        Title = "Digital Marketer",
                        Location = "Athens,Greece",
                        Type = Job.JobType.FullTime,
                        Vacancy = 2,
                        Deadline = DateTime.UtcNow.AddDays(30),
                        Description = @"It is a long established fact that a reader will beff distracted by vbthe creadable content
                            of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf
                            mcore or-lgess normal distribution of letters, as opposed to using, Content here content
                            here making it look like readable."
                    },
                    new Job
                    {
                        Title = "FullStack Developer",
                        Location = "Florida, US",
                        SalaryMin = 60000,
                        SalaryMax =130000,
                        Type = Job.JobType.FullTime,
                        Vacancy = 5,
                        Deadline = DateTime.UtcNow.AddDays(90),
                        Description = @"It is a long established fact that a reader will beff distracted by vbthe creadable content
                            of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf
                            mcore or-lgess normal distribution of letters, as opposed to using, Content here content
                            here making it look like readable."

                    },
                    new Job
                    {
                        Title = "Primary School Teacher",
                        Location = "Florida, US",
                        SalaryMin = 20000,
                        Type = Job.JobType.PartTime,
                        Vacancy = 3,
                        Deadline = DateTime.UtcNow.AddDays(20),
                        Description = @"It is a long established fact that a reader will beff distracted by vbthe creadable content
                            of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf
                            mcore or-lgess normal distribution of letters, as opposed to using, Content here content
                            here making it look like readable."
                    },
                    new Job
                    {
                        Title = "Language Translator",
                        Location = "Florida, US",
                        SalaryMin = 89000,
                        SalaryMax = 2000000,
                        Type = Job.JobType.Remote,
                        Vacancy = 3,
                        Deadline = DateTime.UtcNow.AddDays(17),
                        Description = @"It is a long established fact that a reader will beff distracted by vbthe creadable content
                            of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf
                            mcore or-lgess normal distribution of letters, as opposed to using, Content here content
                            here making it look like readable."
                    },
                    new Job
                    {
                        Title = " Investement Banker",
                        Location = "Florida, US",
                        SalaryMin = 250000,
                        SalaryMax = 400000,
                        Type = Job.JobType.Freelance,
                        Vacancy = 4,
                        Deadline = DateTime.UtcNow.AddDays(25),
                        Description = @"It is a long established fact that a reader will beff distracted by vbthe creadable content
                            of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf
                            mcore or-lgess normal distribution of letters, as opposed to using, Content here content
                            here making it look like readable."
                    }
                };
                await _context.Jobs.AddRangeAsync(jobs);
                await _context.SaveChangesAsync();
            }
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

        public async Task SeedTags()
        {
            if (await _context.Tags.CountAsync() == 0)
            {
                Tag[] tags = {
                    new Tag{Name = "project"},
                    new Tag{Name = "love"},
                    new Tag{Name = "technology"},
                    new Tag{Name = "restaurant"},
                    new Tag{Name = "design"},
                    new Tag{Name = "illustration"},
                };
                await _context.Tags.AddRangeAsync(tags);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedPostsAsync()
        {
            if (await _context.Posts.CountAsync() == 0)
            {
                var postTags = new List<Tag>
                    {
                        new Tag{Name = "travel"},
                        new Tag{Name = "lifestyle"}
                        };
                Post[] posts = {

                    new Post{
                        Title = "FootPrints in Time is Perfect House in Kurashiki",
                        Body = "some random body for the text which is meaningless",
                        Tags =postTags
                    },
                    new Post{
                        Title = "FootPrints in Time is Perfect House in Kurashiki",
                        Body = "some random body for the text which is meaningless",
                        Tags = postTags
                    }
                };
                await _context.Posts.AddRangeAsync(posts);
                await _context.SaveChangesAsync();
            }
        }
    }
}