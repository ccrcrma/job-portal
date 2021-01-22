using System.IO;
using job_portal.Areas.Administration.ViewModels;

namespace job_portal.Areas.Administration.Models
{
    public class Testimonial
    {
        public const string BaseDirectory = "img/testimonials";
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageName)) return string.Empty;
                return Path.Combine(BaseDirectory, ImageName);
            }
        }

        public TestimonialViewModel ToViewModel()
        {
            return new TestimonialViewModel()
            {
                PersonName = Name,
                Designation = Designation,
                Message = Description,
                ImagePath = ImagePath
            };
        }

        public void Update(TestimonialViewModel vm)
        {
            Name = vm.PersonName;
            Designation = vm.Designation;
            Description = vm.Message;
        }
    }
}