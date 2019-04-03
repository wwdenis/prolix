// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Prolix.Core.Collections;
using Prolix.Core.Extensions.Parsing;
using Prolix.Core.Extensions.Reflection;

namespace Prolix.Core.Extensions.Collections
{
    public static class CollectionExtensions
	{
		public static IList<ObjectType> ToList<ObjectType>(this IList list)
		{
			if (list == null)
				return new List<ObjectType>();

			var parsed = list.Cast<ObjectType>();

			return parsed.ToList();
		}

        public static void AddRange<KeyType, ValueType>(this IDictionary<KeyType, ValueType> destination, IDictionary<KeyType, ValueType> source, bool validate = false)
        {
            if (!source?.Any() ?? false || destination == null)
                return;

            foreach (var item in source)
            {
                if (!validate || destination.ContainsKey(item.Key))
                    destination.Add(item.Key, item.Value);
            }   
        }

        public static void AddRange<ItemType>(this ObservableCollection<ItemType> destination, IEnumerable<ItemType> source, bool clear = false)
		{
			if (!source?.Any() ?? false || destination == null)
				return;

			if (clear)
				destination.Clear();

			foreach (var item in source)
				destination.Add(item);
		}

        public static void AddRange<ItemType>(this ICollection<ItemType> destination, IEnumerable<ItemType> source)
        {
            if (!source?.Any() ?? false || destination == null)
                return;

            foreach (var item in source)
                destination.Add(item);
        }

        public static ItemType TryGet<ItemType>(this IEnumerable<ItemType> list, int index = 0)
		{
			if (list == null || index < 0 || index >= list.Count())
				return default(ItemType);

			return list.ElementAtOrDefault(index);
		}

        public static ItemType MoveNext<ItemType>(this IList<ItemType> list, ItemType current = null)
			where ItemType : class
		{
			if (!list?.Any() ?? false)
				return null;

			if (current == null)
				return list.FirstOrDefault();

			var index = list.IndexOf(current);

			if ((index + 1) >= list.Count)
				return null;

			return list[++index];
		}

		public static ItemType MovePrevious<ItemType>(this Collection<ItemType> list, ItemType current = null)
			where ItemType : class
		{
			if (!list?.Any() ?? false)
				return null;

			if (current == null)
				return list.FirstOrDefault();

			var index = list.IndexOf(current);

			if (index <= 0)
				return null;

			return list[--index];
		}

		public static bool IsLast<ItemType>(this Collection<ItemType> list, ItemType current)
			where ItemType : class
		{
			if (!list?.Any() ?? false)
				return false;

			if (current == null)
				return false;

			var index = list.IndexOf(current);

			if (index == list.Count - 1)
				return true;

			return false;
		}

		public static bool IsFirst<ItemType>(this Collection<ItemType> list, ItemType current)
			where ItemType : class
		{
			if (!list?.Any() ?? false)
				return false;

			if (current == null)
				return false;

			var index = list.IndexOf(current);

			if (index == 0)
				return true;

			return false;
		}

		public static TwoWayEnumerator<T> ToTwoWay<T>(this IEnumerable<T> source)
		{
			if (source == null)
				return null;

			return new TwoWayEnumerator<T>(source.GetEnumerator());
		}

		/// <summary>
		/// Returns the index of the specified object in the collection.
		/// </summary>
		/// <param name="list">The collection</param>
		/// <param name="value">The object</param>
		/// <returns>If found returns index otherwise -1</returns>
		public static int IndexOf(this IEnumerable list, object value)
		{
			int index = -1;

			var enumerator = list.GetEnumerator();
			enumerator.Reset();
			int i = 0;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Equals(value))
				{
					index = i;
					break;
				}

				i++;
			}

			return index;
		}

		public static object ElementAt(this IEnumerable list, int ind)
		{
			IEnumerator enumerator = list.GetEnumerator();
			enumerator.Reset();

			object value = null;
			int i = 0;
			while (enumerator.MoveNext())
			{
				if (i == ind)
				{
					value = enumerator.Current;
					break;
				}

				i++;
			}

			return value;
		}

        public static string ToCsv(this IEnumerable list)
        {
            if (list == null)
                return null;

            var builder = new StringBuilder();
            var header = string.Empty;
            
            foreach (object item in list)
            {
                if (string.IsNullOrWhiteSpace(header))
                {
                    var names = item.GetType().GetPropertyNames();

                    if (!names.Any())
                        return string.Empty;

                    header = string.Join(",", names);
                    builder.AppendLine(header);
                }

                var values = item.GetPropertyValues();
                var formatted = from i in values
                                select i?.ToString()?.ParseCsv();

                string line = string.Join(",", formatted);

                builder.AppendLine(line);
            }

            return builder.ToString();
        }

        public static IEnumerable ToEnumerable(this object value)
        {
            if (value == null) return null;
            return value as IEnumerable ?? new[] { value };
        }
    }
}
