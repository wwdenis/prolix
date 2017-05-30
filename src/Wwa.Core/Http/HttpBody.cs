// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Wwa.Core.Collections;
using System;
using System.Collections.Generic;

namespace Wwa.Core.Http
{
	public class HttpBody<ContentType>
    {
		public HttpBody(ContentType content = default(ContentType), IDictionary<string, string> cookies = null)
		{
			Content = content;
			Cookies = new WeakDictionary<string, string>(cookies ?? new Dictionary<string, string>());
		}

		public ContentType Content { get; set; }

		public bool IsForm { get; set; }

		public IDictionary<string, string> Cookies { get; }
	}
}
