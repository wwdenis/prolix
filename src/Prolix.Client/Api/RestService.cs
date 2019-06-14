// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prolix.Collections;
using Prolix.Client.Api;
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
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model</returns>
        async public virtual Task<HttpBody<ResponseType>> Get<ResponseType>(string resource, object param = null)
            where ResponseType : class, new()
        {
            return await GetApi<ResponseType>(resource, param);
        }

        /// <summary>
        /// Performs GET calls to get a list of resources.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="param">The endpoint parameters</param>
        /// <returns>The requested model list</returns>
        async public Task<HttpBody<ResponseType[]>> List<ResponseType>(string resource, object param = null)
            where ResponseType : class
        {
            return await GetApi<ResponseType[]>(resource, param);
		}

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<ResponseType>> Post<ResponseType, BodyType>(string resource, HttpBody<BodyType> body)
			where ResponseType : class
			where BodyType : class
		{
			return await PostApi<ResponseType, BodyType>(resource, body);
		}

        /// <summary>
        /// Performs POST calls to add a new resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<ResponseType>> Post<ResponseType>(string resource, HttpBody<ResponseType> body)
			where ResponseType : class
		{
			return await Post<ResponseType, ResponseType>(resource, body);
		}

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="ResponseType">The result model type</typeparam>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
		async public virtual Task<HttpBody<ResponseType>> Put<ResponseType, BodyType>(string resource, HttpBody<BodyType> body)
            where ResponseType : class
            where BodyType : class
        {
            return await PutApi<ResponseType>(resource, body?.Content);
        }

        /// <summary>
        /// Performs PUT calls to add an existing resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <param name="body">The body data</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<ResponseType>> Put<ResponseType>(string resource, HttpBody<ResponseType> body)
            where ResponseType : class
        {
            return await Put<ResponseType, ResponseType>(resource, body);
        }

        /// <summary>
        /// Performs DELETE calls to remove an existing resource.
        /// </summary>
        /// <typeparam name="BodyType">The body type</typeparam>
        /// <param name="resource">The relative endpoint Url</param>
        /// <returns>The requested response</returns>
        async public virtual Task<HttpBody<ResponseType>> Delete<ResponseType>(string resource)
            where ResponseType : class
        {
            return await DeleteApi<ResponseType>(resource);
        }

        #endregion

        #region Private Methods

        async Task<HttpBody<ResponseType>> GetApi<ResponseType>(string resourceOrUrl, object param)
            where ResponseType : class
        {
			var result = new HttpBody<ResponseType>();
			
			try
            {
				// Builds the url
				string url = param.ToQueryString(resourceOrUrl);

                // Gets JSON and parse the result
                StringBody response = await HttpService.Get(url);
				ResponseType data = JsonConvert.DeserializeObject<ResponseType>(response.Content);
				result = new HttpBody<ResponseType>(data, response);
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

		async Task<HttpBody<ResponseType>> PostApi<ResponseType, BodyType>(string url, HttpBody<BodyType> body)
			where ResponseType : class
			where BodyType: class
		{
			var result = new HttpBody<ResponseType>();
			HttpBody<string> response = null;
			try
			{
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

				var data = JsonConvert.DeserializeObject<ResponseType>(response.Content);
				result = new HttpBody<ResponseType>(data, response);
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

        async Task<HttpBody<ResponseType>> PutApi<ResponseType>(string url, object input)
            where ResponseType : class
        {
            var result = new HttpBody<ResponseType>();

            try
            {
                // Posta o JSON no servidor, e realiza parse do retorno
                var json = JsonConvert.SerializeObject(input);
                var response = await HttpService.Put(url, json);
                var data = JsonConvert.DeserializeObject<ResponseType>(response.Content);
                result = new HttpBody<ResponseType>(data, response);
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


        async Task<HttpBody<ResponseType>> DeleteApi<ResponseType>(string url)
            where ResponseType : class
        {
            var result = new HttpBody<ResponseType>();

            try
            {
                // Recupera o JSON no servidor, e realiza parse do retorno
                var response = await HttpService.Delete(url);
                var data = JsonConvert.DeserializeObject<ResponseType>(response.Content);
                result = new HttpBody<ResponseType>(data, response);
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
