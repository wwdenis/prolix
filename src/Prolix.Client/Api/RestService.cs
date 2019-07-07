// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Prolix.Collections;
using Prolix.Client.Extensions;

namespace Prolix.Client.Api
{
    /// <summary>
    /// Generic REST Client Service
    /// </summary>
    public class RestService : IRestService
    {
        #region Constructors

        public RestService()
        {
        }

        public RestService(string baseUrl, IDictionary<string, string> defaultHeaders)
        {
            BaseUrl = baseUrl;
            DefaultHeaders = defaultHeaders;
        }

        #endregion

        #region Properties

        public string BaseUrl { get; set; }

        public IDictionary<string, string> DefaultHeaders { get; set; } = new WeakDictionary<string, string>();

        IHttpService HttpService => new HttpService(BaseUrl, DefaultHeaders);
		
		#endregion

		#region Static Constructor

		static RestService()
		{
			// Initialize common json configuration
			JsonExtensions.IgnoreErrors();
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs GET calls to get an individual resource.
        /// </summary>
        /// <typeparam name="T">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model</returns>
        async public virtual Task<HttpBody<T>> Get<T>(string resource, object param = null)
            where T : class, new()
        {
            return await GetApi<T>(resource, param);
        }

        /// <summary>
        /// Performs GET calls to get a list of resources.
        /// </summary>
        /// <typeparam name="T">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model list</returns>
        async public Task<HttpBody<T[]>> List<T>(string resource, object param = null)
            where T : class
        {
            return await GetApi<T[]>(resource, param);
		}

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="TR">The result model type</typeparam>
        /// <typeparam name="TB">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<TR>> Post<TR, TB>(string resource, HttpBody<TB> body)
			where TR : class
			where TB : class
		{
			return await PostApi<TR, TB>(resource, body);
		}

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<T>> Post<T>(string resource, HttpBody<T> body)
			where T : class
		{
			return await Post<T, T>(resource, body);
		}

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="TR">The result model type</typeparam>
        /// <typeparam name="TB">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
		async public virtual Task<HttpBody<TR>> Put<TR, TB>(string resource, HttpBody<TB> body)
            where TR : class
            where TB : class
        {
            return await PutApi<TR>(resource, body?.Content);
        }

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<T>> Put<T>(string resource, HttpBody<T> body)
            where T : class
        {
            return await Put<T, T>(resource, body);
        }

        /// <summary>
        /// Performs DELETE calls to remove an existing resource.
        /// </summary>
        /// <typeparam name="T">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<T>> Delete<T>(string resource)
            where T : class
        {
            return await DeleteApi<T>(resource);
        }

        #endregion

        #region Private Methods

        async Task<HttpBody<T>> GetApi<T>(string resourceOrUrl, object param)
            where T : class
        {
            HttpBody<T> result;

            try
            {
				// Builds the url
				string url = param.ToQueryString(resourceOrUrl);

                // Gets JSON and parse the result
                StringBody response = await HttpService.Get(url);
				T data = JsonConvert.DeserializeObject<T>(response.Content);
				result = new HttpBody<T>(data, response);
			}
            catch (HttpException)
            {
                throw;
            }
            catch (Exception ex)
            {
				throw new HttpException(ex);
			}
			
            return result;
        }

		async Task<HttpBody<TR>> PostApi<TR, TB>(string url, HttpBody<TB> body)
			where TR : class
			where TB: class
		{
            HttpBody<TR> result;

			try
			{
                HttpBody<string> response;

				if (body.IsForm)
				{
                    // Post FORM and parse the result
                    var dic = body.ToFormDictionaty();
					response = await HttpService.Post(url, dic);
				}
				else
				{
                    string json = string.Empty;

                    // Post JSON and parse the result
                    if (body.Content != null)
                    {
                        if (body.GetType() == typeof(string))
                            json = body as string;
                        else
                            json = JsonConvert.SerializeObject(body.Content);
                    }

                    response = await HttpService.Post(url, json);
				}

				var data = JsonConvert.DeserializeObject<TR>(response.Content);
				result = new HttpBody<TR>(data, response);
			}
            catch (HttpException)
            {
                throw;
            }
            catch (Exception ex)
			{
				throw new HttpException(ex);
			}

			return result;
		}

        async Task<HttpBody<TR>> PutApi<TR>(string url, object input)
            where TR : class
        {
            HttpBody<TR> result;
            try
            {
                var json = JsonConvert.SerializeObject(input);
                var response = await HttpService.Put(url, json);
                var data = JsonConvert.DeserializeObject<TR>(response.Content);
                result = new HttpBody<TR>(data, response);
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HttpException(ex);
            }

            return result;
        }


        async Task<HttpBody<T>> DeleteApi<T>(string url)
            where T : class
        {
            HttpBody<T> result;
            try
            {
                var response = await HttpService.Delete(url);
                var data = JsonConvert.DeserializeObject<T>(response.Content);
                result = new HttpBody<T>(data, response);
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HttpException(ex);
            }

            return result;
        }

        #endregion
    }
}
