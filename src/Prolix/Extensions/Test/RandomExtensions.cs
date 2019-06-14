// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Extensions.Test
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rnd, int maxValue)
        {
            return rnd.NextDouble(0, maxValue);
        }

        public static double NextDouble(this Random rnd, int minValue, int maxValue)
        {
            var range = rnd.Next(minValue, maxValue);
            return rnd.NextDouble() * range;
        }

        public static DateTime NextDate(this Random rnd, DateTime maxValue)
        {
            return rnd.NextDate(DateTime.MinValue, maxValue);
        }

        public static DateTime NextDate(this Random rnd, DateTime minValue, DateTime maxValue)
        {
            var diff = maxValue - minValue;
            var range = rnd.Next(diff.Days);
            return minValue.AddDays(range);
        }

        public static DateTime NextDate(this Random rnd)
        {
            return rnd.NextDate(DateTime.Now);
        }

        public static DateTime NextTime(this Random rnd, DateTime maxValue)
        {
            return rnd.NextTime(DateTime.MinValue, maxValue);
        }

        public static DateTime NextTime(this Random rnd, DateTime minValue, DateTime maxValue)
        {
            var diff = maxValue - minValue;
            var range = rnd.Next(diff.Seconds);
            return minValue.AddSeconds(range);
        }

        public static DateTime NextTime(this Random rnd)
        {
            var minValue = DateTime.Now - DateTime.Now.TimeOfDay;
            var maxValue = minValue.AddDays(1).AddSeconds(-1);

            return rnd.NextTime(minValue, maxValue);
        }
        public static string NextString(this Random rnd, params string[] args)
        {
            if (!args?.Any() ?? false)
                return string.Empty;

            var index = rnd.Next(args.Length);
            return args[index];
        }

		public static ItemType NextItem<ItemType>(this Random rnd, params ItemType[] args)
		{
			if (!args?.Any() ?? false)
				return default(ItemType);

			var index = rnd.Next(args.Length);
			return args[index];
		}

		public static ItemType NextItem<ItemType>(this Random rnd, List<ItemType> list)
		{
			if (!list?.Any() ?? false)
				return default(ItemType);

			var index = rnd.Next(list.Count - 1);
			var value = list[index];
			list.Remove(value);

			return value;
		}

		public static bool NextBool(this Random rnd)
		{
			var index = rnd.Next(0, 100);
			return index % 2 == 0;
		}

		public static EnumType NextEnum<EnumType>(this Random rnd)
		{
			var type = typeof(EnumType);
			var args = Enum.GetValues(type) as EnumType[];
			var index = rnd.Next(args.Length);
			return args[index];
		}
	}
}
