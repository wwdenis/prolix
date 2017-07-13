// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Wwa.Core.Collections;
using Wwa.Core.Domain;
using Wwa.Core.Extensions.Parsing;
using Wwa.Core.Http;
using Wwa.Http.Extensions;

namespace Wwa.Http.Client
{
    public class ApiService<ModelType, KeyType> : IApiService<ModelType, KeyType>
        where ModelType : Model<KeyType>, new()
        where KeyType : IComparable<KeyType>, IEquatable<KeyType>
    {
        public string BaseUrl { get; set; }
        public string ResourceName { get; set; }

        public IDictionary<string, string> DefaultHeaders { get; set; } = new WeakDictionary<string, string>();

        IRestService RestService => new RestService(BaseUrl, DefaultHeaders);

        async public Task<ModelType> Get(KeyType id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException("ResourceName");

                var url = $"{ResourceName}/{id}";
                var body = await RestService.Get<ModelType>(url);
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

        async public Task<PagedList<ModelType>> List(QueryRequest<ModelType> query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException("ResourceName");

                if (query == null)
                    throw new ArgumentNullException("query");

                var body = await RestService.List<ModelType>(ResourceName, query);
                var result = new PagedList<ModelType>(body?.Content)
                {
                    PageSize = query.PageSize,
                    PageNumber = query.PageNumber,
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

        async public Task<ModelType> Add(ModelType model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException("ResourceName");

                if (model == null)
                    throw new ArgumentNullException("model");

                var request = new HttpBody<ModelType>(model);
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

        async public Task<ModelType> Update(ModelType model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException("ResourceName");

                if (model == null)
                    throw new ArgumentNullException("model");

                var url = $"{ResourceName}/{model.Id}";
                var request = new HttpBody<ModelType>(model);
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

        async public Task Delete(KeyType id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResourceName))
                    throw new ArgumentNullException("ResourceName");

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

    public class ApiService<ModelType> : ApiService<ModelType, int>
        where ModelType : Model, new()
    {
    }
}
