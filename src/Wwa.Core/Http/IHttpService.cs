// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

using Wwa.Core.Ioc;

namespace Wwa.Core.Http
{
    public interface IHttpService : IService
	{
		string BaseUrl { get; set; }
		IDictionary<string, string> DefaultCookies { get; }
		Task<StringBody> Get(string url);
		Task<StringBody> Post(string url, string json);
		Task<StringBody> Post(string url, IDictionary<string, string> form);
		Task<StringBody> Put(string url, string json);
        Task<StringBody> Delete(string url);
    }
}
