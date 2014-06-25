using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parichay.Data.Entity
{
   public static class DateTimeExtension
    {
        public static string Ago(this DateTime targetUTC)
        {
            StringBuilder result = new StringBuilder();
            TimeSpan diff = (DateTime.Now.ToUniversalTime() - targetUTC);

            if (diff.Days > 0)
            {
                result.AppendFormat("{0} days", diff.Days);
            }

            if (diff.Hours > 0)
            {
                if (result.Length > 0)
                {
                    result.Append(", ");
                }

                result.AppendFormat("{0} hours", diff.Hours);
            }

            if (diff.Minutes > 0)
            {
                if (result.Length > 0)
                {
                    result.Append(", ");
                }

                result.AppendFormat("{0} minutes", diff.Minutes);
            }

            if (result.Length == 0)
            {
                result.Append("few moments ago");
            }
            else
            {
                result.Insert(0, "about ");
                result.AppendFormat(" ago");
            }

            return result.ToString();
        }
    }
}
