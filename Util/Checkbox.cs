using static job_portal.Models.Job;

namespace job_portal.Util
{
    public class Checkbox<T> where T : struct
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public bool Selected { get; set; }

    }
}