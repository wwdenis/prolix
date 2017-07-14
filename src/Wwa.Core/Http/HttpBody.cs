// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Wwa.Core.Collections;
using System;
using System.Collections.Generic;

namespace Wwa.Core.Http
{
	public class HttpBody
    {
        public HttpBody()
        {
            Headers = new WeakDictionary<string, string>();
        }

        public bool IsForm { get; set; }

		public IDictionary<string, string> Headers { get; protected set; }
    }

    public class HttpBody<ContentType> : HttpBody
    {
        public HttpBody() : this(default(ContentType))
        {
        }

        public HttpBody(ContentType content)
        {
            Content = content;
        }

        public HttpBody(ContentType content, HttpBody parent) : this(content)
        {
            Headers = parent?.Headers ?? new WeakDictionary<string, string>();
        }

        public ContentType Content { get; set; }
    }
}
