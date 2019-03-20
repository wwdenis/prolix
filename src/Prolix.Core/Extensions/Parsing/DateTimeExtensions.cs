// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Extensions.Parsing
{
	public static class DateTimeExtensions
	{
		readonly static DateTime StartDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static string ToLongString(this DateTime date)
		{
			var format = CultureInfo.CurrentCulture.DateTimeFormat;
			var pattern = format.LongDatePattern;

			if (pattern.StartsWith("DDDD", StringComparison.CurrentCultureIgnoreCase))
			{
				pattern = pattern.Remove(0, 4);
				pattern = pattern.TrimStart(',');
				pattern = pattern.TrimStart(' ');
			}

			return date.ToString(pattern);
		}

        public static string ToDateString(this DateTime date)
        {
            var splitted = date.ToString().Split(' ');

            return splitted[0];
        }

        public static string ToHourString(this DateTime date)
        {
            var splitted = date.ToString().Split(' ');

            return splitted[1] ?? "";
        }

        public static string ToLongString(this DateTime? date)
		{
			if (date == null)
				return string.Empty;

			return date.Value.ToLongString();
		}

		public static DateTime ParseUtc(this long? value)
		{
			return StartDate.AddMilliseconds(value ?? 0);
		}

		public static long ToUtc(this DateTime value)
		{
			var diff = value - StartDate;
			return (long)diff.TotalMilliseconds;
		}

		public static string ToLocalized(this DateTime value, string today, string yesterday, string format = "dd/MM/yyyy")
		{
			var diff = DateTime.Today - value.Date;

			switch (diff.Days)
			{
				case 0:
					return today;
				case 1:
					return yesterday;
			}

			return value.ToString(format);
		}
	}
}
