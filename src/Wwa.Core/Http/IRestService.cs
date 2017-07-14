// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Wwa.Core.Ioc;

namespace Wwa.Core.Http
{
	/// <summary>
	/// Generic REST Client Service
	/// </summary>
	public interface IRestService : IService
    {
        string BaseUrl { get; set; }
        IDictionary<string, string> DefaultHeaders { get; set; }
        

        /// <summary>
        /// Performs GET calls to get an individual resource.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model</returns>
        Task<HttpBody<ResponseType>> Get<ResponseType>(string resource, object param = null)
            where ResponseType : class, new();

        /// <summary>
        /// Performs GET calls to get a list of resources.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model list</returns>
        Task<HttpBody<ResponseType[]>> List<ResponseType>(string resource, object param = null)
			where ResponseType : class;

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<ResponseType>> Post<ResponseType, BodyType>(string resource, HttpBody<BodyType> body)
			where ResponseType : class
			where BodyType : class;

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<ResponseType>> Post<ResponseType>(string resource, HttpBody<ResponseType> body)
			where ResponseType : class;

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<ResponseType>> Put<ResponseType, BodyType>(string resource, HttpBody<BodyType> body)
			where ResponseType : class
			where BodyType : class;

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<ResponseType>> Put<ResponseType>(string resource, HttpBody<ResponseType> body)
            where ResponseType : class;

        /// <summary>
        /// Performs DELETE calls to remove an existing resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<ResponseType>> Delete<ResponseType>(string resource)
            where ResponseType : class;

    }
}
