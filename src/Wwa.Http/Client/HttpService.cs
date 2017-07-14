// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Wwa.Core.Collections;
using Wwa.Core.Extensions.Collections;
using Wwa.Core.Http;
using Wwa.Http.Extensions;

namespace Wwa.Http.Client
{
    public class HttpService : IHttpService
	{
        #region Constructors

        public HttpService()
        {
        }

        public HttpService(string baseUrl, IDictionary<string, string> defaultHeaders)
        {
            BaseUrl = baseUrl;
            DefaultHeaders = defaultHeaders;
        }

        #endregion

        #region Properties

        public string BaseUrl { get; set; }
        
        public IDictionary<string, string> DefaultHeaders { get; set; } = new WeakDictionary<string, string>();

        #endregion

        #region Public Methods

        async public Task<StringBody> Get(string url)
		{
			var content = string.Empty;
			var bodyResponse = new StringBody();
			
			using (var client = CreateClient(url))
            {
                var response = await client.GetAsync(url);

                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

				Trace(url, content, string.Empty);

				if (!response.IsSuccessStatusCode)
                    throw new HttpException(response.ReasonPhrase, response.StatusCode, content);

				var headers = response.GetHeaders();
                bodyResponse = new StringBody(content);
                bodyResponse.Headers.AddRange(headers);
            }
			
            return bodyResponse;
		}

		async public Task<StringBody> Post(string url, string json)
		{
			const string JSON_MEDIA_TYPE = "application/json";

			var content = string.Empty;
			var bodyResponse = new StringBody();

			using (var client = CreateClient(url))
			{
				client.AcceptJson();

				var body = new StringContent(json);
				body.Headers.ContentType = new MediaTypeHeaderValue(JSON_MEDIA_TYPE);
				var response = await client.PostAsync(url, body);

				if (response.Content != null)
				{
					content = await response.Content.ReadAsStringAsync();
				}

				Trace(url, content, json);

				if (!response.IsSuccessStatusCode)
					throw new HttpException(response.ReasonPhrase, response.StatusCode, content);

                var headers = response.GetHeaders();
                bodyResponse = new StringBody(content);
                bodyResponse.Headers.AddRange(headers);
            }

			return bodyResponse;
		}

		async public Task<StringBody> Post(string url, IDictionary<string, string> form)
		{
			if (form == null)
				form = new Dictionary<string, string>();

			var bodyResponse = new StringBody();

			using (var client = CreateClient(url))
			{
				client.AcceptJson();

				var body = new FormUrlEncodedContent(form);
				var response = await client.PostAsync(url, body);

				string content = await response?.Content?.ReadAsStringAsync();
				
				Trace(url, content, form.ToString());

				if (!response.IsSuccessStatusCode)
					throw new HttpException(response.ReasonPhrase, response.StatusCode, content);

                var headers = response.GetHeaders();
                bodyResponse = new StringBody(content);
                bodyResponse.Headers.AddRange(headers);
            }

			return bodyResponse;
		}

		async public Task<StringBody> Put(string url, string json)
        {
            const string JSON_MEDIA_TYPE = "application/json";

            var content = string.Empty;
            var bodyResponse = new StringBody();

            using (var client = CreateClient(url))
            {
				client.AcceptJson();

				var body = new StringContent(json);
                body.Headers.ContentType = new MediaTypeHeaderValue(JSON_MEDIA_TYPE);
                var response = await client.PutAsync(url, body);

                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                Trace(url, content, json);

                if (!response.IsSuccessStatusCode)
                    throw new HttpException(response.ReasonPhrase, response.StatusCode, content);

                var headers = response.GetHeaders();
                bodyResponse = new StringBody(content);
                bodyResponse.Headers.AddRange(headers);
            }

            return bodyResponse;
        }

        async public Task<StringBody> Delete(string url)
        {
            var content = string.Empty;
            var bodyResponse = new StringBody();

            using (var client = CreateClient(url))
            {
                var response = await client.DeleteAsync(url);

                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                Trace(url, content, string.Empty);

                if (!response.IsSuccessStatusCode)
                    throw new HttpException(response.ReasonPhrase, response.StatusCode, content);

                var headers = response.GetHeaders();
                bodyResponse = new StringBody(content);
                bodyResponse.Headers.AddRange(headers);
            }

            return bodyResponse;
        }

        #endregion

        #region Private Methods

        HttpClient CreateClient(string url)
		{
			Uri uri = null;

			if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
			{
				if (string.IsNullOrWhiteSpace(BaseUrl))
					throw new ArgumentNullException("BaseUrl");

				uri = new Uri(BaseUrl);
			}
			
			var cookieContainer = new CookieContainer();
			
			var handler = new HttpClientHandler
			{
				CookieContainer = cookieContainer
			};

			var client = new HttpClient(handler)
			{
				BaseAddress = uri
			};

            if (DefaultHeaders?.Any() ?? false)
            {
                foreach (var header in DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            return client;
		}

		void Trace(string url, string response, string request, [CallerMemberName] string method = null)
		{
#if DEBUG
			if (!Debugger.IsAttached)
				return;

			string line = new string('-', 80);

			Debug.WriteLine(line);
			Debug.WriteLine("DATE: {0}", DateTime.Now);
			Debug.WriteLine("URL: {0}", url);
			Debug.WriteLine("HTTP METHOD: {0}", method?.ToUpper());
			Debug.WriteLine("REQUEST HEADERS: {0}", DefaultHeaders);
			Debug.WriteLine(string.Empty);

			if (!string.IsNullOrWhiteSpace(request))
			{
				Debug.WriteLine("REQUEST:");
				Debug.WriteLine(request);
				Debug.WriteLine(line);
			}

			Debug.WriteLine("RESPONSE:");
			Debug.WriteLine(response);
			Debug.WriteLine(line);
#endif
		}

		#endregion
	}
}
