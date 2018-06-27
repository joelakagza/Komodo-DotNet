using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komodo.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime StartOfMonth(this DateTime theDate)
        {
            return theDate.AddDays(1 - DateTime.Today.Day); ;
        }

        public static DateTime EndOfMonth(this DateTime theDate)
        {
            return theDate = new DateTime(theDate.Year, theDate.Month, DateTime.DaysInMonth(theDate.Year, theDate.Month));
        }


    }
}
