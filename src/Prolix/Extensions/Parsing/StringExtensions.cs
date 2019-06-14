// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Prolix.Extensions.Parsing
{
	public static class StringExtensions
	{
		public static string RemoveAll(this string text, params string[] criteria)
		{
			if (string.IsNullOrWhiteSpace(text))
				return text;

			text = text.Trim();

			foreach (var item in criteria)
				text = text.Replace(item, string.Empty);

			return text;
		}

		public static Dictionary<string, string> ParseQueryString(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return new Dictionary<string, string>();

			text = text.Trim().TrimStart('?');

			var query = from row in text.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
						select row.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

			var result = from i in query
						 where i.Length == 2
						 select i;

			return result.ToDictionary(k => k[0], v => v[1]);
		}

		public static string ToLocalized(this object value, string format, CultureInfo culture = null)
		{
			if (value == null)
				return string.Empty;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

			string template = "{0:" + format + "}";

			return string.Format(culture, template, value);
		}

		public static bool ToBool(this string text, string trueValue = null)
		{
			if (string.IsNullOrWhiteSpace(trueValue))
				trueValue = true.ToString();

			return string.Equals(text, trueValue, StringComparison.CurrentCultureIgnoreCase);
		}

		public static int ToInt(this string text, int defaultValue = 0)
		{
			if (string.IsNullOrWhiteSpace(text))
				return defaultValue;

			int value = defaultValue;

			if (!int.TryParse(text, out value))
				return defaultValue;

			return value;
		}

		public static int ToInt(this char digit, int defaultValue = 0)
		{
			return digit.ToString().ToInt(defaultValue);
		}

		public static int[] ToIntArray(this string digits, bool keepInvalid = false)
		{
			var parsed = from i in digits.ToCharArray()
						 select i.ToInt(-1);

			if (!keepInvalid)
			{
				parsed = parsed.Where(i => i >= 0);
			}

			return parsed.ToArray();
		}

		public static double ToDouble(this string text, CultureInfo culture = null)
		{
			if (string.IsNullOrWhiteSpace(text))
				return 0;

			double value = 0;
			bool success = false;

			if (culture != null)
				success = double.TryParse(text, NumberStyles.Any, culture, out value);
			else
				success = double.TryParse(text, out value);

			return success ? value : 0;
		}

		public static DateTime? ToDateTime(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return null;

			DateTime value = DateTime.MinValue;

			if (!DateTime.TryParse(text, out value))
				return null;

			return value;
		}

		public static string ToTitleCase(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return text;

			var words = text.Split(new[] { " " }, StringSplitOptions.None);
			var result = from word in words
						 select word.ToCapital();

			return string.Concat(result);
		}

		public static string ToCapital(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;

			string first = text.Substring(0, 1).ToUpper();

			if (text.Length == 1)
				return first;
			
			return first + text.Remove(0, 1);
		}

		public static bool IsValidEmail(this string text)
		{
			string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
				+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
				+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

			var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

			return regex.IsMatch(text);
		}

		public static string ParseHtml(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return text ?? string.Empty;

			var mappings = new Dictionary<string, string>()
			{
				{ "<br[^>]*>", "\n" }
			};

			foreach (var map in mappings)
			{
				text = Regex.Replace(text, map.Key, map.Value);
			}

			return text;
		}

		public static long ToLong(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return 0;

			long value = 0;

			string textValue = text.Replace(".", "");
			textValue = textValue.Replace(",", "");

			if (!long.TryParse(textValue, out value))
				return 0;

			return value;
		}

		public static string FormatDigits(this string text)
		{
			return Regex.Replace(text, @"[^\d]", "");
		}

		public static string FormatPhone(this string text)
		{
			if (text == null)
				text = string.Empty;

			int size = text.Length;

			if (size == 1)
				text = Regex.Replace(text, @"(\d{1})", "($1");
			else if (size <= 6)
				text = Regex.Replace(text, @"(\d{2})(\d*)", "($1) $2");
			else if (size <= 10)
				text = Regex.Replace(text, @"(\d{2})(\d{4})(\d*)", "($1) $2-$3");
			else if (size <= 11)
				text = Regex.Replace(text, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");

			return text;
		}

		public static string ParseEnd(this string text, params string[] values)
		{
			if (string.IsNullOrWhiteSpace(text) || !(values?.Any() ?? false))
			{
				return string.Empty;
			}

			foreach (string value in values)
			{
				if (!string.IsNullOrWhiteSpace(value) && value.Length < text.Length && text.EndsWith(value))
				{
					return text.Substring(0, text.Length - value.Length);
				}
			}

			return string.Empty;
		}

        public static string ParseCsv(this string text)
        {
            string[] escapeChars = { ",", "\"", "\n", "\r" };
            bool hasEscaped = escapeChars.Any(i => text.Contains(i));

            if (!hasEscaped)
                return text;

            var escaped = text.Replace("\"", "\"\"");

            return string.Format("\"{0}\"", text);
        }

        public static string ToFriendly(this object value)
        {
            string text = string.Empty;

            if (value == null)
                return string.Empty;

            if (value is bool)
            {
                text = (bool)value ? "Yes" : "No";
            }
            else if (value is DateTime)
            {
                text = string.Format("{0:dd/MM/yyyy HH:mm}", value);
            }
            else
            {
                text = string.Format("{0}", value);
            }

            return text;
        }
    }
}
