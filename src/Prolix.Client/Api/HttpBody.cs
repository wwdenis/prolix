// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Collections;
using System;
using System.Collections.Generic;

namespace Prolix.Client.Api
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

    public class HttpBody<T> : HttpBody
    {
        public HttpBody() : this(default)
        {
        }

        public HttpBody(T content)
        {
            Content = content;
        }

        public HttpBody(T content, HttpBody parent) : this(content)
        {
            Headers = parent?.Headers ?? new WeakDictionary<string, string>();
        }

        public T Content { get; set; }
    }
}
