using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadProvider.Services.Services.Helpers
{
    public static class DateTimeHelper
    {
        private const int SECOND = 1;
        private const int MINUTE = 60 * SECOND;

        public static bool IsLessThenMinute(DateTime dateTime)
        {
            var dateTimeNow = DateTime.Now;
            bool isFuture = (dateTimeNow.Ticks < dateTime.Ticks);
            var ts = dateTimeNow.Ticks < dateTime.Ticks ? new TimeSpan(dateTime.Ticks - dateTimeNow.Ticks) : new TimeSpan(dateTimeNow.Ticks - dateTime.Ticks);

            double delta = ts.TotalSeconds;

            if (delta < 1 * MINUTE || delta < 2 * MINUTE)
            {
                return true;
            }
            return false;
        }
    }
}
