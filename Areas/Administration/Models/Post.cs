using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Models;

namespace job_portal.Areas.Administration.Models
{
    public class Post : PublishableEntity
    {
        public const string PostBaseDirectory = "img/posts";
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string ImageName { get; set; }
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageName)) return string.Empty;
                return Path.Combine(PostBaseDirectory, ImageName);
            }
        }

        public PostViewModel ToViewModel()
        {
            return new PostViewModel
            {
                Id = Id,
                Title = Title,
                Content = Body,
                ImagePath = ImagePath
            };
        }

        public void Update(PostViewModel vm)
        {
            Title= vm.Title;
            Body = vm.Content;
        }


        public string getAllTags
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (Tags != null && Tags.Count > 0)
                {
                    for (int i = 0; i < Tags.Count; i++)
                    {
                        builder.Append(Tags[i].Name);
                        if (i == Tags.Count - 1) continue;
                        builder.Append(", ");
                    }
                }
                return builder.ToString();
            }
        }
    }
}