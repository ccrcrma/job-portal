using System;
using Humanizer;

namespace job_portal.Extensions
{
    public static class DateTimeExtension
    {
        public static string GetHumanFriendlyDate(this DateTime date)
        {
            var numberOfDays = (DateTime.UtcNow.Day - date.Day);
            if (numberOfDays > 30)
            {
                return date.ToString("MMMM dd, yyyy");
            }
            return date.Humanize();
        }
    }
}