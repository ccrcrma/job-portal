namespace job_portal
{
    public static class StringExtensions
    {
        public static string Truncate(this string input, int Length)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (input.Length > Length)
            {
                var subString = input.Substring(0, Length) + "...";
                return subString;
            }
            return input;

        }
    }
}