// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

using Wwa.Core.Extensions.Collections;

namespace Wwa.Api.Cors
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        async public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var policy = BuildPolicy();

            return await Task.FromResult(policy);
        }

        CorsPolicy BuildPolicy()
        {
            const string CORS_CONFIG_SECTION = "corsConfig";
            const string CORS_ORIGINS_KEY = "origins";
            const string CORS_HEADERS_KEY = "headers";
            
            NameValueCollection config = null;

            try
            {
                config = ConfigurationManager.GetSection(CORS_CONFIG_SECTION) as NameValueCollection;
            }
            catch (Exception)
            {
                // Not Found
            }

            if (config == null)
                return null;

            var origins = ParseSettings(config, CORS_ORIGINS_KEY);
            var headers = ParseSettings(config, CORS_HEADERS_KEY);
            
            var policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            if (origins?.Any() ?? false)
                policy.Origins.AddRange(origins);

            if (headers?.Any() ?? false)
                policy.ExposedHeaders.AddRange(headers);

            if (!policy.Origins.Any())
                return null;

            return policy;
        }

        string[] ParseSettings(NameValueCollection list, string key)
        {
            const char CONFIG_SEPARATOR = ',';
            var value = "";

            if (!string.IsNullOrWhiteSpace(key) && (list?.AllKeys?.Contains(key) ?? false))
                value = list[key];

            var parsed = value?.Split(new[] { CONFIG_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            if (parsed == null)
                return new string[0];

            var result = from i in parsed
                         select i.Trim();

            return result.ToArray();
        }
    }
}
