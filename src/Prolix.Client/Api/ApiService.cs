// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Prolix.Collections;
using Prolix.Domain;
using Prolix.Extensions.Parsing;
using Prolix.Client.Api;
using Prolix.Client.Extensions;

namespace Prolix.Client.Api
{
    public abstract class ApiService<TM, TK> : IApiService<TM, TK>
        where TM : Model<TK>, new()
        where TK : IComparable<TK>, IEquatable<TK>
    {
        public string BaseUrl { get; set; }
        public string ResourceName { get; set; }

        public IDictionary<string, string> DefaultHeaders { get; set; } = new WeakDictionary<string, string>();

        IRestService RestService => new RestService(BaseUrl, DefaultHeaders);

        async public Task<TM> Get(TK id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException(nameof(ResourceName));

                var url = $"{ResourceName}/{id}";
                var body = await RestService.Get<TM>(url);
                return body?.Content;
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                ex.CheckRule();
                throw;
            }
        }

        async public Task<PagedList<TM>> List(QueryRequest<TM> query = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException(nameof(ResourceName));

                var body = await RestService.List<TM>(ResourceName, query);
                var result = new PagedList<TM>(body?.Content)
                {
                    PageSize = query?.PageSize ?? 0,
                    PageNumber = query?.PageNumber ?? 1,
                    PageCount = body.Headers["X-Page-Count"]?.ToInt() ?? 0,
                    RecordCount = body.Headers["X-Page-Records"]?.ToInt() ?? 0
                };

                return result;
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                ex.CheckRule();
                throw;
            }
        }

        async public Task<TM> Add(TM model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException(nameof(ResourceName));

                if (model == null)
                    throw new ArgumentNullException(nameof(model));

                var request = new HttpBody<TM>(model);
                var response = await RestService.Post(ResourceName, request);
                return response?.Content;
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                ex.CheckRule();
                throw;
            }
        }

        async public Task<TM> Update(TM model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException(nameof(ResourceName));

                if (model == null)
                    throw new ArgumentNullException(nameof(model));

                var url = $"{ResourceName}/{model.Id}";
                var request = new HttpBody<TM>(model);
                var response = await RestService.Put(url, request);
                return response?.Content;
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                ex.CheckRule();
                throw;
            }
        }

        async public Task Delete(TK id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException(nameof(ResourceName));

                var url = $"{ResourceName}/{id}";
                await RestService.Delete<object>(url);
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return;

                ex.CheckRule();
                throw;
            }
        }
    }

    public abstract class ApiService<T> : ApiService<T, int>
        where T : Model, new()
    {
    }
}
