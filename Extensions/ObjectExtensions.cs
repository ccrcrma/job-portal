namespace job_portal.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetProperty(this object obj, string propertyName)
        {
            return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }
    }
}