using System.Globalization;

namespace Core.Services.TimeServices
{
    public static class TimeService
    {
        public static DateTime GetNow() => DateTime.Now.ToUniversalTime();

        public static DateTime GetMinimum() => DateTime.MinValue;

        public static DateTime GetMaximum() => DateTime.MaxValue;

        public static DateTime? Parse(string date)
        {
            var dateAndTime = date.Substring(0, 24);

            if (DateTime.TryParseExact(dateAndTime, "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
