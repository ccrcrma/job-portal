using System;
using System.Linq;
using static job_portal.ViewModels.SearchFilterViewModel;

namespace job_portal.Util
{
    public static class PostDurationHelper
    {
        public static bool ApplyDurationFilter(PostedDuration[] durations, out int duration)
        {
            if (durations == null || durations.Length == 0) throw new ArgumentNullException("Please enter valid durations");
            duration = 0;
            if (durations.Contains(PostedDuration.Any)) return false;
            foreach (var postCreatedDuration in durations)
            {
                duration = (int)postCreatedDuration > duration ? (int)postCreatedDuration : duration;
            }
            return true;
        }
    }
}