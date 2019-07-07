// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Prolix.Ioc;

namespace Prolix.Client.Api
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
        /// <typeparam name="T">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model</returns>
        Task<HttpBody<T>> Get<T>(string resource, object param = null)
            where T : class, new();

        /// <summary>
        /// Performs GET calls to get a list of resources.
        /// </summary>
        /// <typeparam name="T">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model list</returns>
        Task<HttpBody<T[]>> List<T>(string resource, object param = null)
			where T : class;

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="TR">The result model type</typeparam>
        /// <typeparam name="TB">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<TR>> Post<TR, TB>(string resource, HttpBody<TB> body)
			where TR : class
			where TB : class;

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<T>> Post<T>(string resource, HttpBody<T> body)
			where T : class;

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="TR">The result model type</typeparam>
        /// <typeparam name="TB">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<TR>> Put<TR, TB>(string resource, HttpBody<TB> body)
			where TR : class
			where TB : class;

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<T>> Put<T>(string resource, HttpBody<T> body)
            where T : class;

        /// <summary>
        /// Performs DELETE calls to remove an existing resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <returns>The requested response</returns>
        Task<HttpBody<T>> Delete<T>(string resource)
            where T : class;

    }
}
