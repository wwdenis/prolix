// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prolix.Http.Extensions
{
	public static class JsonExtensions
	{
		public static Exception ParseException { get; set; }

		public static void IgnoreErrors()
		{
			JsonConvert.DefaultSettings = () =>
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					Error = (sender, e) =>
					{
						e.ErrorContext.Handled = true;
						ParseException = e.ErrorContext.Error;

#if DEBUG
                        if (!Debugger.IsAttached)
                        {
                            if (ParseException != null)
                                throw ParseException;
                        }
#endif
                    }
				};
		}

		public static object ToBag(this JToken token)
		{
			var val = token as JValue;
			var arr = token as JArray;
			var obj = token as JObject;

			if (val != null)
				return val.Value;

			if (arr != null)
				return arr.ToBag();

			if (obj != null)
				return obj.ToBag();

			return null;
		}

		public static object[] ToBag(this JArray arr)
		{
			var result = new List<object>();

			foreach (var val in arr.Children())
			{
				var bag = val?.ToBag();
				result.Add(bag);
			}

			return result.ToArray();
		}

		public static Dictionary<string, object> ToBag(this JObject obj)
		{
			var result = new Dictionary<string, object>();

			foreach (var prop in obj.Properties())
			{
				var val = prop?.Value;
				var bag = val?.ToBag();
				result.Add(prop.Name, bag);
			}

			return result;
		}
	}
}
