// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Wwa.Core.Collections;
using Wwa.Core.Domain;

namespace Wwa.Core.Http
{
    /// <summary>
    /// REST Client for Wwa-powered Api's
    /// </summary>
    public interface IApiService<ModelType, KeyType>
        where ModelType : Model<KeyType>, new()
        where KeyType : IComparable<KeyType>, IEquatable<KeyType>
    {
        string BaseUrl { get; set; }
        IDictionary<string, string> DefaultHeaders { get; set; }

        Task<ModelType> Get(KeyType id);
        Task<PagedList<ModelType>> List(QueryRequest<ModelType> query);
        Task<ModelType> Add(ModelType model);
        Task<ModelType> Update(ModelType model);
        Task Delete(KeyType id);
    }

    public interface IApiService<ModelType> : IApiService<ModelType, int>
        where ModelType : Model, new()
    {
    }
}
