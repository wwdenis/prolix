// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Prolix.Core.Collections;
using Prolix.Core.Domain;
using Prolix.Core.Ioc;

namespace Prolix.Client.Api
{
    /// <summary>
    /// REST Client for Prolix-powered Api's
    /// </summary>
    public interface IApiService<ModelType, KeyType> : IService
        where ModelType : Model<KeyType>, new()
        where KeyType : IComparable<KeyType>, IEquatable<KeyType>
    {
        string BaseUrl { get; set; }
        IDictionary<string, string> DefaultHeaders { get; set; }

        Task<ModelType> Get(KeyType id);
        Task<PagedList<ModelType>> List(QueryRequest<ModelType> query = null);
        Task<ModelType> Add(ModelType model);
        Task<ModelType> Update(ModelType model);
        Task Delete(KeyType id);
    }

    public interface IApiService<ModelType> : IApiService<ModelType, int>
        where ModelType : Model, new()
    {
    }
}
