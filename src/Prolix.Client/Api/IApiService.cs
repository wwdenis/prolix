// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Prolix.Collections;
using Prolix.Domain;
using Prolix.Ioc;

namespace Prolix.Client.Api
{
    /// <summary>
    /// REST Client for Prolix-powered Api's
    /// </summary>
    public interface IApiService<TM, TK> : IService
        where TM : Model<TK>, new()
        where TK : IComparable<TK>, IEquatable<TK>
    {
        string BaseUrl { get; set; }
        IDictionary<string, string> DefaultHeaders { get; set; }

        Task<TM> Get(TK id);
        Task<PagedList<TM>> List(QueryRequest<TM> query = null);
        Task<TM> Add(TM model);
        Task<TM> Update(TM model);
        Task Delete(TK id);
    }

    public interface IApiService<T> : IApiService<T, int>
        where T : Model, new()
    {
    }
}
