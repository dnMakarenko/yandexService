using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadProvider.DataImporter
{
    public static class YandexDateTimeHelper
    {
        private const int SECOND = 1;
        private const int MINUTE = 60 * SECOND;

        public static bool isActualData(DateTime dateTime)
        {

            bool isFuture = (DateTime.UtcNow.Ticks < dateTime.Ticks);
            var ts = DateTime.UtcNow.Ticks < dateTime.Ticks ? new TimeSpan(dateTime.Ticks - DateTime.UtcNow.Ticks) : new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);

            double delta = ts.TotalSeconds;

            if (delta < 1 * MINUTE || delta < 2 * MINUTE)
            {
                return true;
            }
            return false;
        }
    }
}
