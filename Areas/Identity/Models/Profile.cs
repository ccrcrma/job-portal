using System;
using System.IO;

namespace job_portal.Areas.Identity.Models
{
    public class Profile
    {
        public const string BaseImageDir = "img/profiles";
        public string Bio { get; set; }
        public string Experience { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public string UserId { get; set; }
        public Guid Id { get; set; }
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageName)) return string.Empty;
                return Path.Combine(BaseImageDir, ImageName);
            }
        }
    }
}