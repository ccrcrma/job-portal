using System;
using System.Collections.Generic;
using System.Text;

namespace job_portal.Models
{
    public class Post : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
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