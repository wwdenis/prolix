// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prolix.Client.Extensions
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
	}
}
