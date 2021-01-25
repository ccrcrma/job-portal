using System;
using job_portal.Types;

namespace job_portal.Areas.Administration.ViewModels
{
    public class JobPostCommonViewModel
    {
        public int Id { get; set; }
        public string EditUrl { get; set; }
        public string DeleteRestoreUrl { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public PublishedStatus Status { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Description { get; set; }

    }
}