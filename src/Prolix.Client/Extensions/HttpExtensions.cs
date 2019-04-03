// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Prolix.Core.Http;
using Prolix.Core.Logic;
using Prolix.Client.Services;

namespace Prolix.Client.Extensions
{
	public static class HttpExtensions
	{
        public static void ChangeAgent(this HttpClient client, string userAgent, bool overrideAgent = false)
		{
			var agent = client?.DefaultRequestHeaders?.UserAgent;
            bool isValid = ProductInfoHeaderValue.TryParse(userAgent ?? string.Empty, out ProductInfoHeaderValue parsedAgent);

            if (agent == null || !isValid)
                return;
            
            if (!agent.Any())
            {
                agent.Add(parsedAgent);
            }
            else if (overrideAgent)
            {
                agent.Clear();
                agent.Add(parsedAgent);
            }
        }

		public static void AcceptJson(this HttpClient client)
		{
			const string JSON_MEDIA_TYPE = "application/json";
			var accept = client?.DefaultRequestHeaders?.Accept;

			if (accept != null && !accept.Any(i => string.Equals(i.MediaType, JSON_MEDIA_TYPE, StringComparison.CurrentCultureIgnoreCase)))
			{
				accept.Add(new MediaTypeWithQualityHeaderValue(JSON_MEDIA_TYPE));
			}
		}

		public static IDictionary<string, string> GetHeaders(this HttpResponseMessage response)
		{
            return response?.Headers?.ToDictionary(k => k.Key, v => string.Join(", ", v.Value)) ?? new Dictionary<string, string>();
		}

        public static IDictionary<string, string> GetCookies(this HttpResponseMessage response)
        {
            var result = new Dictionary<string, string>();

            if (response?.Headers == null || !response.Headers.TryGetValues("set-cookie", out IEnumerable<string> cookies))
                return result;

            foreach (var headerCookie in cookies)
            {
                var rows = headerCookie.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var first = rows.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(first))
                {
                    var cookie = first.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookie.Length == 2)
                        result.Add(cookie[0], cookie[1]);
                }
            }

            return result;
        }

        public static string ToQueryString(this object request, string baseUrl = null)
		{
			var props = request.GetType().GetRuntimeProperties();
			var builder = new StringBuilder();

			foreach (var prop in props)
			{
				var attr = prop.GetCustomAttribute<QueryStringAttribute>();
				var ignore = prop.GetCustomAttribute<ApiIgnoreAttribute>();

				if (ignore == null)
				{
					string name = attr?.Name ?? prop.Name;
					object value = prop.GetValue(request);
					string encoded = WebUtility.UrlEncode(value?.ToString() ?? string.Empty);

					if (!string.IsNullOrEmpty(encoded))
					{
						if (builder.Length > 0)
							builder.Append("&");

						builder.AppendFormat("{0}={1}", name, encoded);
					}
				}
			}

			string queryString = builder.ToString();

			if (string.IsNullOrWhiteSpace(baseUrl))
				return queryString;

			return baseUrl + queryString;
		}

		public static IDictionary<string, string> ToFormDictionaty(this object input)
		{
			var props = input.GetType().GetRuntimeProperties();
			var form = new Dictionary<string, string>();

			foreach (var prop in props)
			{
				var attr = prop.GetCustomAttribute<FormParameterAttribute>();
				var ignore = prop.GetCustomAttribute<ApiIgnoreAttribute>();

				if (ignore == null)
				{
					string name = attr?.Name ?? prop.Name;
					object value = prop.GetValue(input);
					string encoded = value?.ToString() ?? string.Empty;

					if (!string.IsNullOrEmpty(encoded))
					{
						form.Add(name, encoded);
					}
				}
			}

			return form;
		}

        public static void CheckRule(this HttpException ex)
        {
            RuleValidation rule = ex.ParseData<RuleValidation>();

            if (rule != null)
                throw new RuleException(ex.Message, rule);
        }
    }
}
