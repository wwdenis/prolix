// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Extensions.Parsing
{
	public static class ObjectExtensions
	{
		public static bool IsNumeric(this object obj)
		{
			var types = new[] { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(decimal), typeof(float), typeof(double) };
			return obj != null && types.Contains(obj.GetType());
		}

		public static EnumType ToEnum<EnumType>(this object value, EnumType defaultValue)
		{
			return (EnumType)value.ToEnum(typeof(EnumType), defaultValue);
		}

		public static object ToEnum(this object value, Type enumType, object defaultValue)
		{
			if (value is char)
				value = Convert.ToInt32(value);

			if (value == null || !Enum.IsDefined(enumType, value))
				return defaultValue;

			return Enum.ToObject(enumType, value);
		}

		public static bool ToBool(this object value)
		{
			return value != null && value is bool && (bool)value;
		}
	}
}
